using CoreFtp;

namespace PM.InfrastructureModule.Common.Data
{
    public class Ftp
    {
        /// <summary>
        /// Инициализация FTP клиента
        /// </summary>
        /// <returns></returns>
        public static FtpClient FtpClientInitialize()
        {
            FtpClient ftpClient;
            using (ftpClient = new FtpClient(new FtpClientConfiguration
            {
                Host = "vds-z734e6.1gb.ru",
                Username = "user_filestore",
                Password = "LRXNnRwQo1mLlOuSoJBo"
            }))

                return ftpClient;
        }
    }
}
