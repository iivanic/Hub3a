// See https://aka.ms/new-console-template for more information
using System.Diagnostics;

Console.WriteLine ("Generiram PDF417 2D BAR-KOD prema HUB3a standardu.");

string text =
    "HRVHUB30\nHRK\n000000000012355\nŽELJKO SENEKOVIĆ\nIVANEČKA ULICA 125\n42000 VARAŽDIN\n2DBK d.d.\nALKARSKI PROLAZ 13B\n21230 SINJ\nHR1210010051863000160\nHR01\n7269-68949637676-00019\nCOST\nTroškovi za 1. mjesec\n";

Hub3a.Hub3a hub = new Hub3a.Hub3a (text);

string fileName = "test.png";
hub.DajBarKodPNG(fileName);
using
    (var process = Process.Start (
        new ProcessStartInfo {
            FileName = fileName, UseShellExecute = true
        })) {

    }