using JupyterPreProcessor.Core.Cells;
using JupyterPreProcessor.Core.Raw;


var sourceRawCell = new RawCell(new RawLines([

".someMod",
".someMod2 with parameters",
".someMod3 with parameters too",
"",
".not a mod",
"Hello world it is just text",
"1 line more",
"@tag with parameters",
"@second tag",
"@third tag in row",
"Some text between tags",
"",
"@tag2 tag tag",
".not a mod 2",
"Last line"

]), new CellMetadata("Virtual", CellType.Markdown));

var sourceParser = new SourceDocumentCellParser();
var sourceCell = sourceParser.Parse(sourceRawCell);

foreach (var segment in sourceCell.Segments)
{
	Console.WriteLine(segment);
}

Console.WriteLine(new string('=', 50));

var templateRawCell = new RawCell(new RawLines([

"TemplateName",
"some template content",
"",
"${...multiLineParameter}",
"Some text 2",
"Some text ${withSingle} line parameter",
"EOF"

]), new CellMetadata("Virtual", CellType.Markdown));

var templateParser = new TemplateDocumentCellParser();
var templateCell = templateParser.Parse(templateRawCell);

foreach (var segment in templateCell.Segments)
{
	Console.WriteLine(segment);
}