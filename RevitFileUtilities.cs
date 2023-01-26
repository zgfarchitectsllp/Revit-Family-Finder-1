using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ZGF.Revit.RevitFileUtilities
{

    public class RfaFileInfo
    {
        //---------------------------------------
        // M E M B E R S
        //---------------------------------------
        private FileInfo m_family_FileInfo;
        private string m_family_name;
        private string m_category = null;
        private string[] m_family_types = null;
        private bool m_has_type_catalog;
        private XDocument m_xml_data = null;

        //---------------------------------------
        // C O N S T R U C T O R
        //---------------------------------------
        public RfaFileInfo(FileInfo familyFile) // must be RFA file
        {
            if (!familyFile.Extension.Equals(".rfa", StringComparison.InvariantCultureIgnoreCase))
                throw new FileNotFoundException("Must be Revit Family file (.rfa)");

            m_family_FileInfo = familyFile;
            m_family_name = System.IO.Path.GetFileNameWithoutExtension(m_family_FileInfo.Name);

            m_xml_data = ReadRfaHeader(m_family_FileInfo.FullName);

            m_has_type_catalog = File.Exists(m_family_FileInfo.DirectoryName + "\\" + m_family_name + ".txt");

            // <entry xmlns:A="urn:schemas-autodesk-com:partatom" xmlns="http://www.w3.org/2005/Atom">
            //string xn = "{" + m_xml_data.Root.GetDefaultNamespace().NamespaceName + "}";

            XNamespace xns = m_xml_data.Root.GetDefaultNamespace();

            //foreach (XAttribute xa in m_xml_data.Root.Attributes())
            //    Console.WriteLine(xa.Name + " = " + xa.Value);



            //  xmlns = http://www.w3.org/2005/Atom
            //  {http://www.w3.org/2000/xmlns/}A = urn:schemas-autodesk-com:partatom


            // <category> 
            //   <term>23.40.35.17.47.11</term> 
            //   <scheme>std:oc1</scheme> 
            // </category>
            // <category> 
            //   <term>Casework</term> 
            //   <scheme>adsk:revit:grouping</scheme> 
            // </category>

            var categories = from r in m_xml_data.Descendants(xns + "category")
                             where r.Element(xns + "scheme").Value == "adsk:revit:grouping"
                             select new
                             {
                                 term = r.Element(xns + "term").Value,
                             };

            foreach (var r in categories)
            {
                m_category = r.term;
            }


            //foreach (XElement xe in m_xml_data.Root.Elements())
            //    Console.WriteLine(xe.Name);

            if (HasTypeCatalog)
            {
                m_family_types = this.GetFamilyTypesFromTypeCatalog(m_family_FileInfo);
            }
            else
            {

                // {urn:schemas-autodesk-com:partatom}family

                // XAttribute famAtt = m_xml_data.Root.Attribute(xns + "A");
                XElement fam = m_xml_data.Root.Element("{urn:schemas-autodesk-com:partatom}family");

                //int familyTypeCount = Convert.ToInt32(fam.Element("{urn:schemas-autodesk-com:partatom}variationCount").Value);
                //Array.Resize(ref m_family_types, familyTypeCount);

                //foreach (XElement xe in fam.Descendants())
                //{
                //    Console.WriteLine(xe.Name);
                //}

                var types = from r in fam.Descendants(xns + "title")
                            // where r.Element.Name == "{http://www.w3.org/2005/Atom}title"
                            select new
                            {
                                typeName = r.Value,               // xns + "title").Value,
                            };

                Console.WriteLine();
                //int i = 0;
                List<string> theTypes = new List<string>();
                foreach (var r in types)
                {
                    //Debug.WriteLine("type = " + r.typeName);
                    theTypes.Add(r.typeName);
                }

                m_family_types = theTypes.ToArray();

                //{
                //    i++;
                //    Console.WriteLine(i.ToString() + ": Type = " + r.typeName);

                //}
            }

        }
        //---------------------------------------
        // P R O P E R T I E S
        //---------------------------------------
        public string Name { get { return m_family_name; } }
        public string Path { get { return m_family_FileInfo.DirectoryName; } }
        public string FullName { get { return m_family_FileInfo.FullName; } }
        public string Category { get { return m_category; } }
        //public string FamilyType { get; set; } // Should be "user"
        public bool HasTypeCatalog { get { return m_has_type_catalog; } }
        public string[] FamilyTypes { get { return m_family_types; } }

        //---------------------------------------
        // M E T H O D S
        //---------------------------------------
        private string[] GetFamilyTypesFromTypeCatalog(FileInfo TypeCatalogFile)
        {
            //,Width##length##inches,Depth##length##inches,Height##length##inches,Toe Space##length##inches,Counter Thickness##length##inches,File1##Other##,File2##Other##,Description##Other##,Type Comments##other##
            //BJ 15w x 30h,15,24,30,4,1.5,1,1,Base - 2 File,BJ
            //BJ 18w x 30h,18,24,30,4,1.5,1,1,Base - 2 File,BJ

            List<string> theTypes = new List<string>();

            using (StreamReader sr = new StreamReader(m_family_FileInfo.DirectoryName + "\\" + m_family_name + ".txt"))
            {
                try
                {
                    sr.ReadLine(); // Skip, first line is the header

                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        if (null != line)
                        {
                            string t = line.Split(',')[0];
                            if (null != t) theTypes.Add(t);
                        }
                    }
                }
                catch { }
                finally { if (null != sr) sr.Close(); }
            }

            return m_family_types = theTypes.ToArray();
        }

        private string[] GetFamilyTypesFromTypeCatalog(string TypeCatalogFileName)
        {
            return GetFamilyTypesFromTypeCatalog(new FileInfo(m_family_FileInfo.FullName));
        }


        private XDocument ReadRfaHeader(string rfaFile)
        {
            StringBuilder XML = new StringBuilder();

            using (StreamReader sr = new StreamReader(rfaFile, Encoding.UTF8))
            {
                int maxLines = 0;
                string line;

                try
                {
                    while (maxLines < 100)
                    {
                        if (sr.Peek() == 60)
                        {
                            line = sr.ReadLine();
                            if ((null != line) && (line.Length > 50))
                            {
                                if (line.StartsWith("<entry"))
                                {
                                    XML.Append(line);
                                    break;
                                }
                            }
                        }
                        else
                            sr.ReadLine();

                        maxLines++;
                    }
                }
                finally { if (null != sr) sr.Close(); }

                if (XML.Length > 0)
                    return XDocument.Parse(XML.ToString());
                else
                    throw new FileLoadException("XML header data malformed or not found in Revit Family file");
            }
        }
    }

}
