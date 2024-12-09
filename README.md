[简体中文](./README.zh.md) | English

# VLM-OCR-Demo

## A simple example using VLM for OCR tasks and integrating VLM into your own application

## Foreword

In the previous article [TesseractOCR-GUI: Building a Simple and Easy-to-use User Interface for TesseractOCR based on WPF/C#](https://mp.weixin.qq.com/s/t_C360h6AP9P4GfssZnkbA), we built a user interface to facilitate the use of TesseractOCR. Today, we will build a similar interface using Semantic Kernel to connect with a vision model, and test the effectiveness of using a vision model for OCR tasks. In the summary of the previous article [Using Tesseract for Image Text Recognition](https://mp.weixin.qq.com/s/C2o0-RtubtQb4pzys2wx6w), we discussed the drawbacks of using VLM for this task, which were indeed confirmed after testing.

## Effectiveness

Before proceeding to the next step, let's get a general idea of the effectiveness.

Test Image 1:

![](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/SemanticKernel-Test1.png)

Check the Effect:

![image-20241209102333915](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/image-20241209102333915.png)

Test Image 2:

![](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/SemanticKernel-Test2.png)

Check the Effect:

![image-20241209102431184](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/image-20241209102431184.png)

The recognition effect is pretty good when the prompt is well-written.

However, hallucinations still can't be completely avoided:

![image-20241209102824355](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/image-20241209102824355.png)

You need to adjust to the model that provides the best effect.

## For general user usage

Just like the previous software, I have released a compressed package on GitHub. Click to download and then decompress it.

GitHub address: https://github.com/Ming-jiayou/VLM-OCR-Demo

![image-20241209103223078](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/image-20241209103223078.png)

Here, I choose the version of the dependency framework:

![image-20241209103316699](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/image-20241209103316699.png)

After downloading and decompressing, it will look like this:

![image-20241209103826202](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/image-20241209103826202.png)

There is a .env file used to configure the VLM's API Key. This is because my computer's configuration is not powerful enough to run visual models locally with Ollama, so I can only use a large model service provider. Since SiliconCloud still has a quota and is compatible with the OpenAI format, I choose to connect to SiliconCloud here. Currently, there is a promotion for new registrations that gives 20 million tokens, and the best part is that the tokens do not expire. Friends who want to try it can click the link: https://cloud.siliconflow.cn/i/Ia3zOSCU, to register and use.

After registering, copy an API Key:

![image-20241209104442404](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/image-20241209104442404.png)

Open the .env file and enter your API Key. Be careful not to casually disclose your API Key; rest assured, it is stored on your own computer, so I will not know.

As shown below, do not leave any spaces:

![image-20241209104551110](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/image-20241209104551110.png)

Then open VLM-OCR-Demo.exe to start using it!!

I have already written a Prompt for OCR:

![image-20241209104709811](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/image-20241209104709811.png)

Drawbacks may still exist; here it automatically translated into Chinese. You can try again:

![image-20241209104904726](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/image-20241209104904726.png)

It's working fine again, and you can also adjust the Prompt.

Of course, using VLM just for OCR might seem a bit extravagant; OCR is only a basic function of VLM, which can also perform other image-related tasks.

Describe the image:

![](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/Prism.jpg)

![image-20241209105338613](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/image-20241209105338613.png)

Analyze the chart:

![](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/R.png)

![image-20241209105645807](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/image-20241209105645807.png)

More functions can be explored by the readers themselves.

## WPF/C# programmers usage

Fork the project to your own account, git clone it locally, open the solution, and the project structure is as follows:

![image-20241209105907884](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/image-20241209105907884.png)

Since the .env file contains sensitive information such as the API Key, I did not upload it to GitHub. I created a new .env file in the same location with the following format:

```csharp
SILICON_CLOUD_API_KEY=sk-xxx
```

Fill in your own SILICON_CLOUD_API_KEY as shown below:

![image-20241209104551110](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/image-20241209104551110.png)

Set the properties of the .env file:

![image-20241209110301990](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/image-20241209110301990.png)

With these settings, the application should be able to start.

Development tool: Visual Studio 2022

.NET version: .NET 8

Using SemanticKernel makes it very convenient to integrate large language models into our own applications. Previously, I have only integrated chat models and have not yet tried integrating visual models, but the integration process is actually quite simple, as SemanticKernel greatly simplifies the integration operations.

Core code:

```csharp
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
```

This is just a simple demo for learning purposes. The optimal usage can be adjusted according to your specific project requirements, and you can explore the other code on your own.

## Finally

This project is a simple demo that uses VLM for OCR tasks and integrates VLM into your own application using SemanticKernel. It can also serve as a simple practice project for beginners in WPF/C#.

If you find it helpful, giving it a star⭐ is the biggest support!!

If you have any questions, feel free to contact me through my WeChat public account:

![qrcode_for_gh_eb0908859e11_344](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/qrcode_for_gh_eb0908859e11_344.jpg)
