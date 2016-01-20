using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.IO;
using MigraDoc.DocumentObjectModel.Tables;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
//using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;
using PdfSharp.Pdf;
using MigraDoc.Rendering;
using MigraDoc.RtfRendering;

namespace test2
{
    /// <summary>
    /// Логика взаимодействия для ReportWindow1.xaml
    /// </summary>
    public partial class ReportWindow1 : Window
    {
        private TreeViewItem TVItem;

        public ReportWindow1(TreeViewItem item)
        {
            InitializeComponent();

            var document = CreateSample1(item);
            DocumentPreview1.Ddl = DdlWriter.WriteToString(document);

            TVItem = item;
        }

        public Document CreateSample1(TreeViewItem item)
        {
            Connection connection = new Connection();
            connection.Open();

            MySqlCommand myCommand = new MySqlCommand(@"
SELECT
DATE_FORMAT(defectsdata.DatePr,'%Y-%m-%d'),
worksmens.NameSmen,
sizetubes.SizeTube,
gosts.NameGost,
controlsamples.NameControlSample,
o1.Surname,
o1.LevelMD,
o2.Surname,
o2.LevelMD,
device.NameDevice,
defectsdata.Porog,
defectsdata.Current,
Count(defectsdata.IndexData),
(   SELECT
    Count(defectsdata.IndexData)
    FROM
    defectsdata
    Inner Join indexes ON defectsdata.IndexData = indexes.IndexData
    Inner Join worksmens ON worksmens.Id_WorkSmen = indexes.Id_WorkSmen
    WHERE
    defectsdata.FlDefectTube = 1
        AND
    defectsdata.NumberTube <> 0
        AND
    defectsdata.DatePr = '2013-12-25'
        AND
    indexes.Id_WorkSmen = 3
)
FROM defectsdata
Inner Join indexes ON defectsdata.IndexData = indexes.IndexData
Inner Join worksmens ON worksmens.Id_WorkSmen = indexes.Id_WorkSmen
Inner Join sizetubes ON sizetubes.Id_SizeTube = indexes.Id_SizeTube
Inner Join gosts ON gosts.Id_Gost = indexes.Id_Gost
Inner Join controlsamples ON controlsamples.Id_ControlSample = indexes.Id_ControlSample
Inner Join operators AS o1 ON o1.Id_Operator = indexes.Id_Operator1
Inner Join operators AS o2 ON o2.Id_Operator = indexes.Id_Operator2
Inner Join device ON device.Id_Device = indexes.Id_Device
WHERE
DATE_FORMAT(defectsdata.DatePr, '%Y-%m-%d') = @A
    AND
indexes.Id_WorkSmen = @B
    AND
defectsdata.NumberTube <> 0
GROUP BY
    defectsdata.DatePr
LIMIT 1
", connection.mySqlConnection);
            myCommand.Parameters.Clear();
            myCommand.Parameters.AddWithValue("A", item.Uid.Split('|')[0]);
            myCommand.Parameters.AddWithValue("B", item.Uid.Split('|')[1]);

            MySqlDataReader dataReader = myCommand.ExecuteReader();

            //==
            Document document = new Document();
            // section
            var section = document.AddSection();
            // paragraf
            var paragraph = section.AddParagraph();

            while (dataReader.Read())
            {
                Title = "Отчет за " + dataReader.GetString(0) + " по смене " + dataReader.GetString(1);
                paragraph.AddText("ПАО \"Северский трубный завод\"\r\nТЭСЦ-2 стан 73-219");
                paragraph.Format.Font.Size = "14";
                paragraph.Format.Alignment = ParagraphAlignment.Center;

                paragraph = section.AddParagraph();
                paragraph = section.AddParagraph();

                paragraph.AddText("Сводная информация по смене");
                paragraph.Format.Font.Size = "12";
                paragraph.Format.Alignment = ParagraphAlignment.Center;

                document.LastSection.AddParagraph();
                Table table = new Table();
                table.Format.Font.Size = "11";
                table.Borders.Width = 0.5;

                var column = table.AddColumn(Unit.FromCentimeter(8));
                table.AddColumn(Unit.FromCentimeter(10));
                var padding = 8;

                //===============
                var row = table.AddRow();
                row.BottomPadding = padding;
                Cell cell = row.Cells[0];
                cell.AddParagraph("Дата");
                cell = row.Cells[1];
                cell.AddParagraph(dataReader.GetString(0));
                //================
                row = table.AddRow();
                row.BottomPadding = padding;
                cell = row.Cells[0];
                cell.AddParagraph("смена");
                cell = row.Cells[1];
                cell.AddParagraph(dataReader.GetString(1));
                //=====================
                row = table.AddRow();
                row.BottomPadding = padding;
                cell = row.Cells[0];
                cell.AddParagraph("Типоразмер труб");
                cell = row.Cells[1];
                cell.AddParagraph(dataReader.GetString(2));
                //===================
                row = table.AddRow();
                row.BottomPadding = padding;
                cell = row.Cells[0];
                cell.AddParagraph("Нормативные документы");
                cell = row.Cells[1];
                cell.AddParagraph(dataReader.GetString(3));
                //===============================
                row = table.AddRow();
                row.BottomPadding = padding;
                cell = row.Cells[0];
                cell.AddParagraph("Контрольный образец");
                cell = row.Cells[1];
                cell.AddParagraph(dataReader.GetString(4));
                //========================
                row = table.AddRow();
                row.BottomPadding = padding;
                cell = row.Cells[0];
                cell.AddParagraph("Специалист ОКПП");
                cell = row.Cells[1];
                cell.AddParagraph(dataReader.GetString(5));
                //=======================
                row = table.AddRow();
                row.BottomPadding = padding;
                cell = row.Cells[0];
                cell.AddParagraph("Уровень по МПР (FT)");
                cell = row.Cells[1];
                cell.AddParagraph(dataReader.GetString(6));
                //=======================
                row = table.AddRow();
                row.BottomPadding = padding;
                cell = row.Cells[0];
                cell.AddParagraph("Специалист АСК ТЭСЦ-2");
                cell = row.Cells[1];
                cell.AddParagraph(dataReader.GetString(7));
                //==================
                row = table.AddRow();
                row.BottomPadding = padding;
                cell = row.Cells[0];
                cell.AddParagraph("Уровень по МПР (FT)");
                cell = row.Cells[1];
                cell.AddParagraph(dataReader.GetString(8));
                //==================
                row = table.AddRow();
                row.BottomPadding = padding;
                cell = row.Cells[0];
                cell.AddParagraph("Параметры настройки");
                cell = row.Cells[1];
                cell.AddParagraph();
                //=========================
                row = table.AddRow();
                row.BottomPadding = padding;
                cell = row.Cells[0];
                cell.AddParagraph("Автоматический контроль шва");
                cell = row.Cells[1];
                cell.AddParagraph();
                //=========================
                row = table.AddRow();
                row.BottomPadding = padding;
                cell = row.Cells[0];
                cell.AddParagraph("Установка");
                cell = row.Cells[1];
                cell.AddParagraph(dataReader.GetString(9));
                //======================
                row = table.AddRow();
                row.BottomPadding = padding;
                cell = row.Cells[0];
                cell.AddParagraph("Порог, мВ");
                cell = row.Cells[1];
                cell.AddParagraph(dataReader.GetString(10));
                //============================
                row = table.AddRow();
                row.BottomPadding = padding;
                cell = row.Cells[0];
                cell.AddParagraph("Ток, А");
                cell = row.Cells[1];
                cell.AddParagraph(dataReader.GetString(11));
                //========================
                row = table.AddRow();
                row.BottomPadding = padding;
                cell = row.Cells[0];
                cell.AddParagraph("Проконтролировано труб");
                cell = row.Cells[0];
                cell.AddParagraph(dataReader.GetString(12));
                //======================
                row = table.AddRow();
                row.BottomPadding = padding;
                cell = row.Cells[0];
                cell.AddParagraph("Дефектных труб");
                cell = row.Cells[1];
                cell.AddParagraph(dataReader.GetString(13));

                document.LastSection.Add(table);
            }

            dataReader.Close();
            connection.Close();

            return document;

        }

        private void Button_exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Button_SavePdf_Click(object sender, RoutedEventArgs e)
        {
            const bool unicode = true;
            const PdfFontEmbedding embedding = PdfFontEmbedding.Always;
            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(unicode, embedding);

            pdfRenderer.Document = CreateSample1(TVItem);

            pdfRenderer.RenderDocument();
            // save
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = "ТЭСЦ-2 стан 73-219 " + Title;
            dlg.DefaultExt = ".pdf";
            var st = (bool)dlg.ShowDialog();
            if (st)
            {
                pdfRenderer.PdfDocument.Save(dlg.FileName);
            }
        }

        
        private void Button_SaveRtf_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName= "ТЭСЦ-2 стан 73-219 " + Title;
            dlg.DefaultExt = ".rtf";
            var st = (bool)dlg.ShowDialog();
            if (st)
            {
                var rtf = new RtfDocumentRenderer();
                rtf.Render(CreateSample1(TVItem), dlg.FileName, null);
            }
        }
    }
}
