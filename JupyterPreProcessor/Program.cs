using JupyterPreProcessor;
using JupyterPreProcessor.Core.Engine;
using JupyterPreProcessor.Core.Raw;

var sourceDocument = new RawDocument([

	new RawCell(new RawLines([
		"[Parameters]",
		"",
		"[DefaultMods]",
		""

	]), new("Virtual/Source.ipynb", CellType.Markdown)),


	new RawCell(new RawLines([
		".demo",
		"",
		"Other text 1",
		"@test",
		"@testTemplate axaxaxa",
		"Other text 2"

	]), new("Virtual/Source.ipynb", CellType.Markdown))
]);

var templateDocument = new RawDocument([

	new RawCell(new RawLines([
		"Example",
		"Multi line template",
		"Data from 'data' parameter: ${data}, post chars",
		""

	]), new("Virtual/Template.ipynb", CellType.Markdown))
]);


var engine = new DefaultPreprocessorEngine();

engine.RegisterPlugin(new TestPlugin());
engine.SetTemplateDocument(templateDocument);

var resultDocument = engine.Process(sourceDocument);

var i = 0;
foreach (var rawCell in resultDocument.Cells)
{
	Console.WriteLine($"[{i}] \"{rawCell.Metadata.SourceDocument}\" {rawCell.Metadata.Type}:");
	foreach (var line in rawCell.Lines.Lines)
		Console.WriteLine("|" + line);
	i++;
}