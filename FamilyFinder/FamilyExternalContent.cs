using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.ApplicationServices;
using System.Windows.Forms;

namespace ZGF.Revit
{
#pragma warning disable 0169, 0649, 0044
    /// <summary>
    /// Wrapper for a Family Type, used for sorting EXTERNAL items in browser window
    /// </summary>    
    public class RevitContent_External_Utilities : IComparable
    {
        // https://www.codeproject.com/Articles/43025/A-LINQ-Tutorial-Mapping-Tables-to-Objects

        // M E M B E R S

        public const string SQL_SERVER_HOSTNAME = "PDX-SQL-3";

        public const string CONNECTION_STRING =
            "Data Source=" + SQL_SERVER_HOSTNAME + ";Initial Catalog=ZGF_Revit_Content;Integrated Security=True;" +
            "Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;" +
            "ApplicationIntent=ReadWrite;MultiSubnetFailover=False";


        private int m_ID;
        private string m_FolderSearchTerm;
        private string m_Category;
        private string m_FileName;
        private string m_FullPathName;
        private int m_FileVersion;
        //private RevitFileTypes m_FileType;
        private string m_FileType;
        private string m_Description;
        private Image m_previewImage;
        //private Image m_userEditableImage;

        private string m_FullSearchTerm;


        // C O N S T R U C T O R S

        public RevitContent_External_Utilities(Revit_Content_EXTERNAL_Item dataRecord) //, RevitFileTypes contentFileType)
        {
            m_Description = (null == dataRecord.Description) ? string.Empty : dataRecord.Description;
            m_FolderSearchTerm = (null == dataRecord.FolderSearchTerm) ? string.Empty : dataRecord.FolderSearchTerm;

            m_ID = dataRecord.ID;
            m_Category = dataRecord.CategoryName;
            m_FileName = dataRecord.FileName;
            m_FullPathName = dataRecord.FullPathName;
            m_FileVersion = dataRecord.FileVersion;

            // TODO: Link table in Content DB
            // Database Table:
            //      ID = Int
            //      Name = String
            //      FileExtension = String
            //m_FileType = contentFileType;

            

            m_FileName = dataRecord.FileType;

            // TODO: Need compiler directive here for resource specific to VS Project:
            //m_previewImage = (null == dataRecord.PreviewImage) ? ConsoleApp1.Properties.Resources.No_preview_200x200 : dataRecord.PreviewImage;

            // Concatenate the full search term:
            StringBuilder fst = new StringBuilder(string.Empty);
            if (!String.IsNullOrEmpty(FolderSearchTerm)) fst.Append(FolderSearchTerm);
            fst.Append(FileName);
            fst.Append(FileType);
            fst.Append(CategoryName);
            if (!String.IsNullOrEmpty(Description)) fst.Append(Description);

            m_FullSearchTerm = fst.ToString();


        }

        // P R O P E R T I E S

        public int ID { get { return m_ID; } }
        public string FolderSearchTerm { get { return m_FolderSearchTerm; } }
        public string FileName { get { return m_FileName; } }
        public string FileType { get { return m_FileType; } }
        public int FileVersion { get { return m_FileVersion; } }
        public string FullPathName { get { return m_FullPathName; } }
        public string CategoryName { get { return m_Category; } }
        public string Description { get { return m_Description; } }
        public Image PreviewImage { get { return m_previewImage; } }
        public Image UserEditableImage { get { return ZGF.Revit.Properties.Resources.EditFamily; } }

        public string FullSearchTerm { get { return m_FullSearchTerm; } }


        /// <summary>
        /// Name of database where content paths live.
        /// </summary>
        public static string RevitContentDataConnectionString(string sqlServerName)
        {
            return
                    "Data Source=" +
                    sqlServerName +
                    ";Initial Catalog=ZGF_Revit_Content;Persist Security Info=True;User ID=RevitContentUser;Password=RevitContentUser";
        }

        /// <summary>
        /// Queries ZGF Content Database and returns all unique records, by FileName, from the specified
        /// Revit version downward, inclusive.
        /// </summary>
        /// <param name="currentVersion"></param>
        /// <returns></returns>
        public static List<Revit_Content_EXTERNAL_Item> GetRevitExternalContentItems(int currentVersion)
        {
            try
            {
                long replyTime;
                if (ZGF.Server.ServerConnectionTester.TestServerConnection(
                    RevitContent_External_Utilities.SQL_SERVER_HOSTNAME, 1500, out replyTime))
                {
                    ZGF_Revit_Content db = new ZGF_Revit_Content();

                    IEnumerable<Revit_Content_EXTERNAL_Item> records = db.ContentTableData
                                        .Where(x => x.FileVersion <= currentVersion)
                                        .OrderByDescending(x => x.FileVersion)
                                        .GroupBy(v => v.FileName)
                                        .Select(x => x.First());

                    return new List<Revit_Content_EXTERNAL_Item>(records);
                }
                else
                {
                    string message = "The ZGF Revit family library database is currently unavailable.";
                    string content = "Are you connected to the network?"
                        + "\nIf out of the office, are you connected to the VPN?"
                        + "\n\nIf above checks out, please contact Technology.";

                    TaskDialog td = new TaskDialog("ZGF Revit Family Finder");
                    td.MainInstruction = message;
                    td.MainContent = content;
                    td.TitleAutoPrefix = false;
                    td.Show();

                    return new List<Revit_Content_EXTERNAL_Item>();
                }
            }
            catch (Exception ex)
            {
                string message = "The ZGF Revit family library database is not accessible. Are you connected to the VPN?";
                string content = "Please contact Technology. Error code: " + ex.Message;

                TaskDialog td = new TaskDialog("ZGF Revit Family Finder");
                td.MainInstruction = message;
                td.MainContent = content;
                td.TitleAutoPrefix = false;
                td.Show();

                //return new List<Revit_Content_EXTERNAL_Item>();
                return null;
            }
        }




        public static void InsertDraftingView(UIDocument revitDoc, Revit_Content_EXTERNAL_Item externalDataRecord)
        {
#if DEBUG
            exContent_Views.InsertDraftingView(revitDoc, externalDataRecord);
#else
            _stub(externalDataRecord);
#endif
        }

        public static void InsertSchedule(UIDocument revitDoc, Revit_Content_EXTERNAL_Item externalDataRecord)
        {
            exContent_Views.InsertSchedule(revitDoc, externalDataRecord);
        }
        public static void InsertSheet(UIDocument revitDoc, Revit_Content_EXTERNAL_Item externalDataRecord)
        {
            _stub(externalDataRecord);
        }

        public static void InsertMaterial(UIDocument revitdoc, Revit_Content_EXTERNAL_Item externalDataRecord)
        {
            _stub(externalDataRecord);
        }

        private static void _stub(Revit_Content_EXTERNAL_Item record)
        {
            StringBuilder msg = new StringBuilder("Data:\n\n");
            msg.Append("\t" + record.FileType + "\n");
            msg.Append("\t" + record.CategoryName + "\n");
            msg.Append("\t" + record.FileName + "\n");

            MessageBox.Show(msg.ToString(), "This feature is under construction");
        }



        public bool IsMatch(string searchWords)
        {
            char[] delimiters = new char[] { ' ', System.Convert.ToChar(160) };
            string stringToMatch = this.FullSearchTerm.ToLower();
            string literal = searchWords.ToLower();
            string[] words = literal.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

            int matches = 0;

            foreach (string s in words)
            {
                if (literal == string.Empty) continue;

                if ((stringToMatch.Contains(literal)) || (stringToMatch.Contains(s)))
                    matches++;
                else
                    matches--;
            }

            return (matches == words.Length);
        }


        public int CompareTo(object obj)
        {
            RevitContent_External_Utilities item = (RevitContent_External_Utilities)obj;
            return String.Compare(
                (this.FullSearchTerm),
                (item.FullSearchTerm));
        }

    }

    [Database(Name = "ZGF_Revit_Content")]
    public class ZGF_Revit_Content : DataContext
    {
        public Table<Revit_Content_EXTERNAL_Item> ContentTableData;

#if DEBUGx
        public ZGF_Revit_Content() : base(RevitContent_External_Utilities.CONNECTION_STRING) { }
#else
        public ZGF_Revit_Content() : base(RevitContent_External_Utilities.RevitContentDataConnectionString(RevitContent_External_Utilities.SQL_SERVER_HOSTNAME)) { }
#endif

    }




    [Table(Name = "RevitContentMain")]
    public class Revit_Content_EXTERNAL_Item : IComparable
    {
        // P R O P E R T I E S

        [Column(IsPrimaryKey = true)]
        public int ID { get; set; }
        [Column]
        public string FolderSearchTerm { get; set; }
        [Column]
        public string FileName { get; set; }
        [Column]
        public string FileType { get; set; }
        [Column]
        public int FileVersion { get; set; }
        [Column]
        public string FullPathName { get; set; }
        [Column]
        public string CategoryName { get; set; }
        [Column]
        public string Description { get; set; }
        [Column]
        public Image PreviewImage { get; set; }
     
        public Image UserEditableImage { get; set; }

        //[Column]
        public string FullSearchTerm
        {
            get
            {
                return FolderSearchTerm + " " + FileName + " " + FileType + " " + CategoryName + " "
                      + ((null == Description) ? string.Empty : Description);
            }
        }

        /// <summary>
        /// This function should only be called on a Loadable Family (because they're the only kind that support Type Catalogs.
        /// </summary>
        /// <returns></returns>
        public FileInfo GetTypeCatalog_FileInfo()
        {
            if (FileType.Equals("Loadable Family"))
            {
                return new FileInfo(Path.Combine(
                    Path.GetDirectoryName(FullPathName),
                    Path.GetFileNameWithoutExtension(FullPathName) + ".txt"));
            }
            else
                return null;
        }

        public bool IsMatch(string searchWords)
        {
            char[] delimiters = new char[] { ' ', System.Convert.ToChar(160) };
            string stringToMatch = this.FullSearchTerm.ToLower();
            string literal = searchWords.ToLower();
            string[] words = literal.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

            int matches = 0;

            foreach (string s in words)
            {
                if (literal == string.Empty) continue;

                if ((stringToMatch.Contains(literal)) || (stringToMatch.Contains(s)))
                    matches++;
                else
                    matches--;
            }

            return (matches == words.Length);
        }


        public int CompareTo(object obj)
        {
            Revit_Content_EXTERNAL_Item item = (Revit_Content_EXTERNAL_Item)obj;
            return String.Compare(
                (this.FullSearchTerm),
                (item.FullSearchTerm));
        }

       

    }

    [Table]
    public class RevitFileTypes
    {
        [Column]
        public int ID { get; set; }
        [Column]
        public string Name { get; set; }
        [Column]
        public string FileExtension { get; set; }
    }

}
