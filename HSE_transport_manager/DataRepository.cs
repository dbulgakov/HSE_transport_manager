using System.IO;
using System.Xml.Serialization;
using HSE_transport_manager.Common.Models;

namespace HSE_transport_manager
{
    class DataRepository : IDataRepository
    {
        private const string FileName = "keys.xml";

        public KeyData KeyData
        {
            get
            {
                return ReadXmlFile();
            }
            set
            {
                WriteXmlFile(value);
            }
        }

        private KeyData ReadXmlFile()
        {
            XmlSerializer formatter = new XmlSerializer(typeof(KeyData));
            KeyData keyData = null;
            using (FileStream fs = new FileStream(FileName, FileMode.OpenOrCreate))
            {
                keyData = (KeyData)formatter.Deserialize(fs);
            }
            return keyData;
        }


        private void WriteXmlFile(KeyData keyData)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(KeyData));
            using (FileStream fs = new FileStream(FileName, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, keyData);
            }
        }
    }
}
