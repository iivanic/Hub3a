// See https://aka.ms/new-console-template for more information

// -------------------
//    Hub3a example
// -------------------

using System.Diagnostics;

Console.WriteLine("Generiram PDF417 2D BAR-KOD prema HUB3a standardu.");

//if you have prepared hub3a record
string text =
    "HRVHUB30\nHRK\n000000000012355\nŽELJKO SENEKOVIĆ\nIVANEČKA ULICA 125\n42000 VARAŽDIN\n2DBK d.d.\nALKARSKI PROLAZ 13B\n21230 SINJ\nHR1210010051863000160\nHR01\n7269-68949637676-00019\nCOST\nTroškovi za 1. mjesec\n";
Hub3a.Hub3a hub = new Hub3a.Hub3a(text);

//or set properties
hub = new Hub3a.Hub3a();
hub.Iznos = (123.55 * 100).ToString();
hub.Opis = "Troškovi za 1. mjesec.";
hub.PlatiteljPrvaLinija = "ŽELJKO SENEKOVIĆ";
hub.PlatiteljDrugaLinija = "IVANEČKA ULICA 125";
hub.PlatiteljTrecaLinija = "42000 VARAŽDIN";
hub.PrimateljPrvaLinija = "2DBK d.d.";
hub.PrimateljDrugaLinija = "ALKARSKI PROLAZ 13B";
hub.PrimateljTrecaLinija = "21230 SINJ";
hub.PrimateljIBAN = "HR1210010051863000160";
hub.PozivNaBroj = "7269-68949637676-00019";
hub.Model = "HR01";

// You can generate file on disk
string fileName = "test.svg";
hub.DajBarKodSVG(fileName);
openFile(fileName);

fileName = "test.png1";
hub.DajBarKodPNG(fileName);
openFile(fileName);

fileName = "test.pdf";
hub.DajPDFUplatnicu(fileName);
openFile(fileName);

// or use Stream to get file
using (Stream pdfStream = hub.DajPDFUplatnicu())
{
    // do somesthing with Stream...
}


using (Stream pngStream = hub.DajBarKodPNG())
{
    // do somesthing with Stream...
}

//SVG returns string, not stream
string svgString = hub.DajBarKodSVG();
// do something with string...


//helper func to show file in defualt app
static void openFile(string filename)
{
    Console.WriteLine($"Pritisnite bilo koju tipku za pokušaj otvaranja datoteke {filename}{System.Environment.NewLine}..." );
    Console.ReadKey(true);
        using
            (var process = Process.Start(
                new ProcessStartInfo
                {
                    FileName = filename,
                    UseShellExecute=true
                }))
        {
            if(process==null)
            Console.WriteLine(
                $"Datoteka nije odmah otvorena. Možda nemate aplikaciju za otvaranje ove vrste datoteke ({System.IO.Path.GetExtension(filename)}).");
        }  
}
