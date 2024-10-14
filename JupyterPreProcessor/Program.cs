using JupyterSharpParser;
using JupyterSharpParser.Renderers.Json;

var inputContent = File.ReadAllText("input.ipynb");
var inputDocument = Jupyter.Parse(inputContent);



{
	using var outputWriter = new StreamWriter(File.Open("output.ipynb", FileMode.Create, FileAccess.Write));
	var renderer = new JsonRenderer(outputWriter);
	renderer.Render(inputDocument);
}
