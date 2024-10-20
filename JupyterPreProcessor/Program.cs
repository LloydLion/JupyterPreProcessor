using JupyterPreProcessor;
using JupyterPreProcessor.Core.Engine;
using JupyterPreProcessor.Core.Raw;
using JupyterPreProcessor.Plugins;
using JupyterSharpParser;
using JupyterSharpParser.Renderers.Json;
using JupyterSharpParser.Syntax;
using JupyterSharpParser.Syntax.Cell.Common;


var sourceDocumentData = File.ReadAllText("source.ipynb");
var sourceDocument = load(sourceDocumentData, "source.ipynb");


var templateDocumentData = File.ReadAllText("template.ipynb");
var templateDocument = load(templateDocumentData, "template.ipynb");


var engine = new DefaultPreprocessorEngine();

engine.RegisterPlugin(new TitlePagePlugin());
engine.RegisterPlugin(new ContentTablePlugin());
engine.RegisterPlugin(new WrapPlugin());

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

var resultDocumentData = save(resultDocument);
File.WriteAllText("output.ipynb", resultDocumentData);


RawDocument load(string data, string source)
{
	var document = Jupyter.Parse(data);
	var cells = document.Cells
		.Select(s => s switch
		{
			JupyterSharpParser.Syntax.Cell.MarkdownCell md =>
				new { Lines = md.Source, Type = CellType.Markdown },

			JupyterSharpParser.Syntax.Cell.CodeCell code =>
				new { Lines = code.Source, Type = CellType.Code },

			_ => throw new NotSupportedException()
		})
		.Select(s => new RawCell(new RawLines(s.Lines), new CellMetadata(source, s.Type)))
		.ToArray();

	return new RawDocument(cells);
}

string save(RawDocument document)
{
	var jDocument = new JupyterDocument
	{
		Cells = document.Cells
			.Select<RawCell, JupyterSharpParser.Syntax.Cell.ICell>(s => s.Metadata.Type switch
			{
				CellType.Markdown => new JupyterSharpParser.Syntax.Cell.MarkdownCell()
				{ Source = toLines(s.Lines) },

				CellType.Code => new JupyterSharpParser.Syntax.Cell.CodeCell()
				{ Source = toLines(s.Lines) },

				_ => throw new NotSupportedException()
			})
			.ToArray()
	};

	var writer = new StringWriter();
	var renderer = new JsonRenderer(writer);
	renderer.Render(jDocument);
	return writer.ToString();

	Lines toLines(RawLines rawLines)
	{
		var lines = new Lines();
		lines.AddRange(rawLines.Lines);
		return lines;
	}
}
