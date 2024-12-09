简体中文|[English](./README.md) 

# VLM-OCR-Demo

## 一个使用VLM用于OCR任务与将VLM接入自己应用的简单示例

## 前言

上一篇文章[TesseractOCR-GUI：基于WPF/C#构建TesseractOCR简单易用的用户界面](https://mp.weixin.qq.com/s/t_C360h6AP9P4GfssZnkbA)中我们构建了一个方便使用TesseractOCR的用户界面，今天构建一个类似的界面，使用Semantic Kernel接入视觉模型，测试一下用视觉模型做OCR任务的效果。在之前的文章[使用Tesseract进行图片文字识别](https://mp.weixin.qq.com/s/C2o0-RtubtQb4pzys2wx6w)的总结中说了使用VLM做这个任务的缺点，经过测试之后，发现确实存在。

## 效果

在进行下一步之前，先大概了解一下效果。

测试图片1：

![](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/SemanticKernel-Test1.png)

查看效果：

![image-20241209102333915](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/image-20241209102333915.png)

测试图片2：

![](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/SemanticKernel-Test2.png)

查看效果：

![image-20241209102431184](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/image-20241209102431184.png)

在写好提示词的情况下，识别的效果还不错。

但是还是不免会出现幻觉：

![image-20241209102824355](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/image-20241209102824355.png)

需要自己调整到效果最好的模型。

## 普通用户使用

跟之前的软件一样，我已经在GitHub发布了压缩包，点击下载，然后解压即可。

GitHub地址：https://github.com/Ming-jiayou/VLM-OCR-Demo

![image-20241209103223078](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/image-20241209103223078.png)

这里我选择依赖框架的版本：

![image-20241209103316699](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/image-20241209103316699.png)

下载解压之后如下所示：

![image-20241209103826202](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/image-20241209103826202.png)

有一个.env文件，用于配置VLM的API Key。这是因为我电脑的配置不太行，无法本地用Ollama跑视觉模型，因此只能使用大模型服务商的。由于SiliconCloud还有额度，并且兼容了OpenAI格式，因此我这里选择接入SiliconCloud。现在注册有送2000万token的活动，最nice的一点是送的token没有时间期限。想试试的朋友可以点击链接：https://cloud.siliconflow.cn/i/Ia3zOSCU，注册使用。

注册之后，复制一个API Key：

![image-20241209104442404](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/image-20241209104442404.png)

打开.env填入API Key，注意API Key不要随意泄露，放心这是存储在自己的电脑上，我不会知道。

如下所示，不要留空格：

![image-20241209104551110](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/image-20241209104551110.png)

然后打开VLM-OCR-Demo.exe即可使用啦！！

我已经写好了一个用于OCR的Prompt：

![image-20241209104709811](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/image-20241209104709811.png)

缺点还是会存在，这里自动翻译成了中文，可以再试一下：

![image-20241209104904726](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/image-20241209104904726.png)

又正常了，也可以重新调整一下Prompt。

当然VLM如果只是用于OCR有点太奢侈了，OCR只是VLM的一个基础功能，还可以执行其他与图像有关的任务。

描述图片：

![](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/Prism.jpg)

![image-20241209105338613](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/image-20241209105338613.png)

分析图表：

![](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/R.png)

![image-20241209105645807](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/image-20241209105645807.png)

更多功能可由读者自行探索。

## WPF/C# 程序员使用

将项目Fork到自己账号下，git clone 到本地，打开解决方案，项目结构如下：

![image-20241209105907884](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/image-20241209105907884.png)

由于.env文件包含API Key这个敏感信息，因此我没有上传到GitHub上，自己在同样的位置新建一个.env文件，格式如下所示：

```csharp
SILICON_CLOUD_API_KEY=sk-xxx
```

填入自己的SILICON_CLOUD_API_KEY，如下所示：

![image-20241209104551110](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/image-20241209104551110.png)

设置.env文件的属性：

![image-20241209110301990](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/image-20241209110301990.png)

这样设置一下，应该就可以启动了。

开发工具：Visual Studio 2022

.NET版本：.NET 8

使用SemanticKernel很方便我们接入大语言模型到我们自己的应用中，之前只接入过对话模型，还没有试过接入视觉模型，其实接入也很简单，SemanticKernel大大简化了接入操作。

核心代码：

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

这只是一个简单的Demo，可供学习使用，具体的最佳使用方式，可根据自己的项目需求调整，其他代码可自行探索。

## 最后

本项目是一个使用VLM用于OCR任务与使用SemanticKernel将VLM接入自己应用的简单Demo，对WPF/C#新手程序员，也可以当作一个简单的练手小项目。

如果对你有所帮助，点颗star⭐，就是最大的支持！！

还有任何问题，欢迎通过微信公众号与我联系：

![qrcode_for_gh_eb0908859e11_344](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/qrcode_for_gh_eb0908859e11_344.jpg)
