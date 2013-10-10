using System;
using System.IO;
using System.IO.IsolatedStorage;

namespace PhoneCoreLibrary.Storage
{
    /// <summary>
    /// （独立存储区）文件操作服务类
    /// 注意：使用完毕释放资源
    /// </summary>
    public class IsolatedStorageFileService : IDisposable
    {
        private readonly IsolatedStorageFile _store;

        public IsolatedStorageFileService()
        {
            _store = IsolatedStorageFile.GetUserStoreForApplication();
        }

        public Stream OpenFile(string path, FileMode mode, FileAccess fileAccess)
        {
            return _store.OpenFile(path, mode, fileAccess);
        }

        public void CreateDirectory(string path)
        {
            if (!_store.DirectoryExists(path))
            {
                _store.CreateDirectory(path);
            }
        }

        public bool DirectoryExists(string path)
        {
            return _store.DirectoryExists(path);
        }

        public bool FileExists(string filePath)
        {
            return _store.FileExists(filePath);
        }

        /// <summary>
        /// move文件
        /// </summary>
        /// <param name="sourceFullFileName">原文件完整路径</param>
        /// <param name="destinationFileName">目的文件名</param>
        /// <param name="directory">目的文件夹,eg有："/downloads"，无：null</param>
        public void Movefile(string sourceFullFileName, string destinationFileName, string directory)
        {
            if (directory != null && !DirectoryExists(directory))
            {
                _store.CreateDirectory(directory);
            }

            if (!_store.FileExists(sourceFullFileName)) return;

            var fullPath = directory == null ? destinationFileName : Path.Combine(directory, destinationFileName);
            if (_store.FileExists(fullPath))
            {
                _store.DeleteFile(fullPath);
            }

            _store.MoveFile(sourceFullFileName, fullPath);
        }

        public void RemoveAll()
        {
            _store.Remove();
        }

        /// <summary>
        /// 删除指定文件
        /// </summary>
        /// <param name="path"></param>
        public void Delete(string path)
        {
            if (_store.FileExists(path))
            {
                _store.DeleteFile(path);
            }
        }

        public void Dispose()
        {
            if (_store != null)
                _store.Dispose();
        }

        /// <summary>
        /// 保存文本文件
        /// </summary>
        /// <param name="fullPath"></param>
        /// <param name="content"></param>
        public void SaveFile(string fullPath, string content)
        {
            if (FileExists(fullPath))
            {
                Delete(fullPath);
            }
            using (var isoFileStream = new IsolatedStorageFileStream(fullPath, FileMode.Create, _store))
            {
                var streamWriter = new StreamWriter(isoFileStream);
                streamWriter.Write(content);
                streamWriter.Close();
            }
        }

        /// <summary>
        /// 保存流文件
        /// </summary>
        /// <param name="fullPath">文件完整路径</param>
        /// <param name="stream">数据流</param>
        public void SaveFile(string fullPath, Stream stream)
        {
            if (FileExists(fullPath))
            {
                Delete(fullPath);
            }

            using (var fileStream = new IsolatedStorageFileStream(fullPath, FileMode.Create, _store))
            {
                if (stream != null)
                {
                    stream.CopyTo(fileStream);
                }
            }
        }

        /// <summary>
        /// wp 删除独立存储空间文件（多级非空文件夹删除） 
        /// </summary>
        /// <param name="filePath"></param>
        public void DeleteDirectory(string filePath) //unZipFilePath第一次传递的是根目录名
        {
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (store.DirectoryExists(filePath))
                {
                    String[] dirNames = store.GetDirectoryNames(string.Concat(filePath, "\\*"));
                    String[] fileNames = store.GetFileNames(string.Concat(filePath, "\\*"));
                    if (fileNames.Length > 0)
                    {
                        for (int i = 0; i < fileNames.Length; i++)
                        {
                            store.DeleteFile(string.Concat(filePath, "\\", fileNames[i]));
                        }
                    }
                    if (dirNames.Length == 0)
                    {
                        store.DeleteDirectory(filePath);
                        if (filePath != null && filePath.IndexOf("\\") != -1)
                        {
                            filePath = filePath.Substring(0, filePath.LastIndexOf("\\"));
                            DeleteDirectory(filePath);
                        }
                    }
                    if (dirNames.Length > 0)
                    {
                        for (int i = 0; i < dirNames.Length; i++)
                        {
                            DeleteDirectory(string.Concat(filePath, "\\", dirNames[i]));
                        }
                    }
                }
            }
        }
    }
}
