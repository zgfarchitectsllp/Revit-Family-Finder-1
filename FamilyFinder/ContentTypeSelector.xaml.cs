using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Infragistics.Windows.DataPresenter;


namespace ZGF.Revit
{
    /// <summary>
    /// Interaction logic for ContentTypeSelector.xaml
    /// </summary>
    public partial class ContentTypeSelector : Window
    {
        private FileInfo _TypeCatalogFile;
        private char[] splitCharsData = new char[] { ',' };
        private List<HeaderDataItem> _headerDataItems = new List<HeaderDataItem>();
        private Dictionary<string, List<CellDataItem>> _gridData = new Dictionary<string, List<CellDataItem>>();
        private Dictionary<string, List<CellDataItem>> _filtered_dicSelectedItems = new Dictionary<string, List<CellDataItem>>();
        private DataTable _tableData = new DataTable("gridData");
        private bool returnValue = false;

        // Deploy Infragistics dependencies:
        // https://www.infragistics.com/help/wpf/developers-guide-deploying-your-application

        private MySortableBindingList<string> tcData;

        public int SelectedRecordCount { get { return _filtered_dicSelectedItems.Count; } }

        public Dictionary<string, List<CellDataItem>> SelectedTypeItems { get { return _filtered_dicSelectedItems; } }

        public bool ReturnValue { get { return returnValue; } }

        // Typical heading string: 
        //      ,A##area##inches,d##length##inches,b##length##inches
        public ContentTypeSelector(string TypeCatalogFilePath)
        {
            InitializeComponent();

            _TypeCatalogFile = new FileInfo(TypeCatalogFilePath);

            this.Width += this.Width / 2;

            // does file exist?
            if (_TypeCatalogFile.Exists)
                tcData = new MySortableBindingList<string>();
            else
                throw new FileNotFoundException("Cannot open \"" + _TypeCatalogFile.FullName + "\"");

            string familyName = System.IO.Path.GetFileNameWithoutExtension(_TypeCatalogFile.Name);
            this.Title = "Load Family Types - " + familyName;

            // Read File
            using (StreamReader sr = new StreamReader(_TypeCatalogFile.FullName))
            {
                // C O L U M N S
                //
                // 1st Row is heading:

                // 1st Column in TXT has no name:
                DataColumn dCol = new DataColumn("Type");
                _tableData.Columns.Add(dCol); //<--The Row header?                
                dCol.Unique = true;

                char[] trimValChars = new char[] { '\"' };

                // Remaining Columns
                string[] _headings = sr.ReadLine().Split(splitCharsData, StringSplitOptions.None);

                for (int i = 1; i < _headings.Length; i++)
                {
                    // "i - 1" - Remember that first item in header is for Type Name, and is always blank in the .TXT file.
                    DataGridTextColumn col = new DataGridTextColumn();
                    _headerDataItems.Add(new HeaderDataItem(_headings[i]));
                    col.Header = _headerDataItems[i - 1];
                    dCol = new DataColumn(_headerDataItems[i - 1].FieldName);
                    _tableData.Columns.Add(dCol);
                }

                //
                //  R O W S
                //

                // Read remaining rows:
                while (!sr.EndOfStream)
                {
                    string dataText = sr.ReadLine().Trim();
                    if (string.IsNullOrEmpty(dataText)) continue;

                    string[] valData = dataText.Split(splitCharsData);

                    for (int i = 0; i < valData.Length; i++)
                    {
                        // remove any quote chars
                        valData[i] = valData[i].Trim(trimValChars);
                    }

                    if (valData.Length != _headerDataItems.Count + 1) continue; // Bad string. Incorrect number of data items. TODO: Need a way to log this...

                    //string rowHeaderTypeName = valData[0];
                    List<CellDataItem> _rowData = new List<CellDataItem>();
                    for (int i = 1; i < valData.Length; i++)
                    {
                        CellDataItem cdi = new CellDataItem(_headerDataItems[i - 1], valData[i]);
                        try
                        {
                            _rowData.Add(new CellDataItem(_headerDataItems[i - 1], valData[i]));
                        }
                        catch (Exception ex) { string msg = ex.Message; } // <--Probably not a unique type name
                    }

                    // The Dictionary of data:
                    _gridData.Add(valData[0], _rowData);
                    // The grid view. 
                    _tableData.LoadDataRow(valData, LoadOption.OverwriteChanges);
                }

                sr.Close();

            }

            // Display Infragistics data grid:
            datagridInfra.DataSource = _tableData.DefaultView;
            datagridInfra.SelectedDataItem = _tableData.DefaultView.Table.Rows[0];

            // Display WPF data grid:            
            //datagridTypes.ItemsSource = _tableData.DefaultView;
            //datagridTypes.SelectedIndex = 0;


        }


        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            _filtered_dicSelectedItems.Clear();


            Record[] drs = datagridInfra.Records.Where(r => r.IsSelected).ToArray();
            foreach (Record rec in drs)
            {
                DataRecord dr = rec as DataRecord;
                string sKey = dr.Cells[0].Value.ToString();
                _filtered_dicSelectedItems.Add(sKey, _gridData[sKey]);
            }

            returnValue = true;

            if (null == drs || drs.Length == 0)
                this.Close(); //<--Throw error?
            else
                this.Hide();

            //foreach (DataRowView x in datagridTypes.SelectedItems)
            //{
            //    DataRow dataRow = x.Row;

            //    string sKey = dataRow.ItemArray[0].ToString();
            //    _filtered_dicSelectedItems.Add(sKey, _gridData[sKey]);

            //}

            //returnValue = true;

            //if (_filtered_dicSelectedItems.Count > 0)
            //    this.Hide();
            //else
            //    this.Close();


        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {

            _filtered_dicSelectedItems.Clear();

            returnValue = false;
        }
    }

    public class HeaderDataItem
    {
        // Typical heading string: 
        //      ,A##area##inches,d##length##inches,b##length##inches
        private string[] splitCharsHeaderItem = new string[] { "##" };

        public HeaderDataItem(string headerItemString)
        {
            string[] hdr = headerItemString.Split(splitCharsHeaderItem, StringSplitOptions.None);

            FieldName = hdr[0];
            DataType = hdr[1];
            Units = hdr[2];
        }

        public string FieldName { get; internal set; }
        public string DataType { get; internal set; }
        public string Units { get; internal set; }

        public override string ToString()
        {
            return this.FieldName;
        }

    }



    public class CellDataItem
    {
        public CellDataItem(HeaderDataItem headerItem, string valueString)
        {
            HeaderItem = headerItem;
            ValueString = valueString;
        }

        public HeaderDataItem HeaderItem { get; internal set; }

        public string ValueString { get; internal set; }

        public int ValueAsInteger() { return Convert.ToInt32(ValueString); }
        public double ValueAsDouble() { return Convert.ToDouble(ValueString); }

        public string ParameterName { get { return HeaderItem.FieldName; } }

        public string DataType { get { return HeaderItem.DataType; } }
        public string UnitType { get { return HeaderItem.Units; } }

        public override string ToString()
        {
            return this.ValueString;
        }
    }




}
