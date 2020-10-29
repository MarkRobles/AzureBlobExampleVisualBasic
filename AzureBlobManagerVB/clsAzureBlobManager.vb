Imports System.IO
Imports Azure.Storage.Blobs
Imports Azure.Storage.Blobs.Models

Public Class clsAzureBlobManager
    Public Function Main() As Task
        Console.WriteLine("Azure Blob storage v12 - .NET  C# Console quickstart sample" & vbLf)
        Dim connectionString As String = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING")
        Console.WriteLine(String.Concat("connectionString:", connectionString))
        'Crear nuevo contenedor
        Dim blobServiceClient As BlobServiceClient = New BlobServiceClient(connectionString)
        Dim containerName As String = "test" & Guid.NewGuid().ToString()
        Dim containerClient As BlobContainerClient = blobServiceClient.CreateBlobContainer(containerName)
        'Subir Archivo a contenedor
        Dim localPath As String = "./data/"
        Dim fileName As String = "test" & Guid.NewGuid().ToString() & ".txt"
        Dim localFilePath As String = Path.Combine(localPath, fileName)
        File.WriteAllText(localFilePath, "Hello, World!")
        Dim blobClient As BlobClient = containerClient.GetBlobClient(fileName)
        Console.WriteLine("Uploading to Blob storage as blob:" & vbLf & vbTab & " {0}" & vbLf, blobClient.Uri)

        Using uploadFileStream As FileStream = File.OpenRead(localFilePath)
            blobClient.Upload(uploadFileStream, True)
            uploadFileStream.Close()
        End Using
        'Mostrar archivos en contenedor
        Console.WriteLine("Listing blobs...")

        For Each blobItem As BlobItem In containerClient.GetBlobs()
            Console.WriteLine(vbTab & blobItem.Name)
        Next
        'Descargar archhivo
        Dim downloadFilePath As String = localFilePath.Replace(".txt", "DOWNLOADED.txt")
        Console.WriteLine(vbLf & "Downloading blob to" & vbLf & vbTab & "{0}" & vbLf, downloadFilePath)
        Dim download As BlobDownloadInfo = blobClient.Download()

        Using downloadFileStream As FileStream = File.OpenWrite(downloadFilePath)
            download.Content.CopyTo(downloadFileStream)
            downloadFileStream.Close()
        End Using
        'Eliminar contenedor y sus archivos
        Console.Write("Press any key to begin clean up")
        Console.ReadLine()
        Console.WriteLine("Deleting blob container...")
        containerClient.Delete()
        Console.WriteLine("Deleting the local source and downloaded files...")
        File.Delete(localFilePath)
        File.Delete(downloadFilePath)



        Console.WriteLine("Done")
        Console.ReadLine()
    End Function
End Class
