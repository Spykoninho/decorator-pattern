using PatternDecoratorExample;

Application.Main();

namespace PatternDecoratorExample
{
    public interface IDataSource
    {
        public void WriteData(string content);
        public string ReadData();
    }

    public class FileDataSource : IDataSource
    {
        private string _filename;
        private string _content;

        public FileDataSource(string filename)
        {
            _filename = filename;
            Console.WriteLine($"Création du fichier {_filename}");
        }

        public void WriteData(string content)
        {
            _content = content;
            Console.WriteLine($"écriture dans le fichier {_filename} : {_content}");
        }

        public string ReadData()
        {
            return _content;
        }
    }

    public abstract class DataSourceDecorator : IDataSource
    {
        protected readonly IDataSource _wrappee;

        public DataSourceDecorator(IDataSource source)
        {
            _wrappee = source;
        }

        public virtual void WriteData(string content)
        {
            _wrappee.WriteData(content);
        }

        public virtual string ReadData()
        {
            return _wrappee.ReadData();
        }
    }

    public class CompressionDecorator : DataSourceDecorator
    {
        public CompressionDecorator(IDataSource source) : base(source)
        {
        }

        public override void WriteData(string content)
        {
            string compressedString = $"<compressed>{content}</compressed>";

            base.WriteData(compressedString);
        }

        public override string ReadData()
        {
            string data = base.ReadData();
            string decompressed = $"<decompressed>{data}</decompressed>";
            return decompressed;
        }
    }
    
    public class EncryptionDecorator : DataSourceDecorator
    {
        public EncryptionDecorator(IDataSource source) : base(source)
        {
        }

        public override void WriteData(string content)
        {
            string encryptedString = $"<encrypted>{content}</encrypted>";
            base.WriteData(encryptedString);
        }

        public override string ReadData()
        {
            string data = base.ReadData();
            string decryptedData = $"<decrypted>{data}</decrypted>";
            return decryptedData;
        }
    }

    class Application
    {
        public static void Main()
        {
            IDataSource source = new FileDataSource("fichier.txt");
            
            source.WriteData("Bonjour monsieur");

            Console.WriteLine(source.ReadData());

            source = new CompressionDecorator(source);
            
            source.WriteData("Très longue phrase");
            
            source = new EncryptionDecorator(source);
            
            source.WriteData("Phrase secrète");
            
            Console.WriteLine(source.ReadData());
        }
    }

}

