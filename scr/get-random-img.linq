<Query Kind="Program">
  <NuGetReference>Mapster</NuGetReference>
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Net</Namespace>
</Query>

//下载随机图片

async void Main()
{

	//需要获取几张图？
	var num = 100;

	//需要获取的图片大小？
	var size = (310, 155);
	
	//需要获取的图片格式？
	var format = "jpg";

	var http = new HttpClient();
	http.DefaultRequestHeaders.Referrer = new Uri("https://picsum.photos");
	http.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/102.0.0.0 Safari/537.36");

	for (var i = 0; i != num; ++i)
	{
		Console.WriteLine($"{i + 1}/{num}");
		var bytes = await http.GetByteArrayAsync($"https://picsum.photos/{size.Item1}/{size.Item2}.{format}");
		File.WriteAllBytes($"{i}.{format}", bytes);
	}


}

