ArgParse
========

概要
----

ごくシンプルなコマンドライン引数解析ライブラリ

現在のところ、以下のものは扱えない

 * 値を伴わないオプション (特定機能のオンオフのためのオプション --verbose)
 * position 引数
 * サブコマンド

使用例
------

    var parser = new ArgParser(desc: "simple example");
    try
    {
        parser.AddArgument<string>("--host", "localhost");
        parser.AddArgument<int>("--port", 80);
    
	    Dictionary<string, object> args = parser.Parse("--host 192.168.0.1 --port 8080".Split(' '));
    
        System.Console.WriteLine((string)args["host"]);
        System.Console.WriteLine((int)args["port"]);
    }
    catch (ArgumentException e)
    {
        System.Console.WriteLine(e.Message);
    }
