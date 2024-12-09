using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.IO;
using dotenv.net;
using Prism.Mvvm;
using Prism.Commands;

namespace VLM_OCR_Demo.ViewModels
{
#pragma warning disable SKEXP0010
    public class VLMChatViewModel : BindableBase
    {
        public VLMChatViewModel()
        {
            SelectImageCommand = new DelegateCommand(ExecuteSelectImageCommand);
            AskAICommand = new DelegateCommand(async () => await ExecuteAskAICommand());
            AskAIText = "请仅输出识别的文本，不要包含任何其他信息。请以最简洁的方式输出结果，不要添加任何额外内容。纯文本格式即可。";
            DotEnv.Load();
            var envVars = DotEnv.Read();
            SiliconCloudAPIKey = envVars["SILICON_CLOUD_API_KEY"];
        }

        private string? _selectedFilePath;
        public string? SelectedFilePath
        {
            get { return _selectedFilePath; }
            set { SetProperty(ref _selectedFilePath, value); }
        }

        private string? _askAIText;
        public string? AskAIText
        {
            get { return _askAIText; }
            set { SetProperty(ref _askAIText, value); }
        }

        private string? _askAIResponse;
        public string? AskAIResponse
        {
            get { return _askAIResponse; }
            set { SetProperty(ref _askAIResponse, value); }
        }

        public List<string> VLMOptions { get; set; } = new List<string> { "Pro/OpenGVLab/InternVL2-8B", "OpenGVLab/InternVL2-26B",
                                                                          "OpenGVLab/InternVL2-Llama3-76B","Qwen/Qwen2-VL-72B-Instruct",
                                                                          "Pro/Qwen/Qwen2-VL-7B-Instruct","TeleAI/TeleMM"};
        private string? _selectedVLM;
        public string? SelectedVLM
        {
            get { return _selectedVLM; }
            set { SetProperty(ref _selectedVLM, value); }
        }

        public string? SiliconCloudAPIKey { get; set; }
        public ICommand SelectImageCommand { get; private set; }
        public ICommand AskAICommand { get; private set; }

        private void ExecuteSelectImageCommand()
        {
            // 创建 OpenFileDialog 实例
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.png, *.bmp)|*.jpg;*.jpeg;*.png;*.bmp|All files (*.*)|*.*",
                Title = "选择图片文件"
            };

            // 显示对话框
            if (openFileDialog.ShowDialog() == true)
            {
                // 获取选中的文件路径
                SelectedFilePath = openFileDialog.FileName;
            }
        }

        private async Task ExecuteAskAICommand()
        {
            if (AskAIResponse != "")
            {
                AskAIResponse = "";
            }
            if (SelectedVLM == null)
            {
                SelectedVLM = "Pro/OpenGVLab/InternVL2-8B";
            }
            IKernelBuilder kernelBuilder = Kernel.CreateBuilder();
            kernelBuilder.AddOpenAIChatCompletion(
                modelId: SelectedVLM,
                apiKey: SiliconCloudAPIKey,
                endpoint: new Uri("https://api.siliconflow.cn/v1")
            );
            Kernel kernel = kernelBuilder.Build();

            var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

            if (SelectedFilePath == null)
            {
                return;
            }

            byte[] bytes = File.ReadAllBytes(SelectedFilePath);

            // Create a chat history with a system message instructing
            // the LLM on its required role.
            var chatHistory = new ChatHistory("你是一个描述图片的助手，全程使用中文回答");

            // Add a user message with both the image and a question
            // about the image.
            chatHistory.AddUserMessage(
              [
                new TextContent(AskAIText),
                new ImageContent(bytes, "image/jpeg"),
              ]);

            // Invoke the chat completion model.        
            var response = chatCompletionService.GetStreamingChatMessageContentsAsync(
                                        chatHistory: chatHistory,
                                        kernel: kernel
                                    );
            await foreach (var chunk in response)
            {
                AskAIResponse += chunk;
            }
        }
    }
}
