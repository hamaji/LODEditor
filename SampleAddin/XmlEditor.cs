using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EnvDTE80;
using EnvDTE;

namespace SampleAddin
{
    public partial class XmlEditor : Form
    {
        DTE2 _applicationObject;
        private DataSet _dataSet;
        private string _xmlFilePath;

        public XmlEditor(DTE2 applicationObject)
        {
            // 呼び出し元のConnectクラスから
            // アプリケーション・オブジェクトを引数で受け取り、
            // それをフィールド変数にセットします
            _applicationObject = applicationObject;

            InitializeComponent();
        }

        private void XmlEditor_Load(object sender, EventArgs e)
        {
            // ［ソリューション エクスプローラ］で現在選択されている
            // ファイルのコレクションを取得します
            SelectedItems items = _applicationObject.SelectedItems;

            // ファイルのコレクションの中から1つ目のアイテムを取得します
            SelectedItem item = items.Item(1);

            // ファイル・パスを取得してフィールド変数にセットします
            _xmlFilePath = item.ProjectItem.get_FileNames(1);
            _dataSet = new DataSet();
            try
            {
                // データセットにファイルを読み込み、
                // 内容をデータグリッドビューに表示します
                _dataSet.ReadXml(_xmlFilePath);
                xmlDataGridView.DataSource = _dataSet.Tables[0];

                // 読み込んだファイル名をラベルに表示します
                fileNameLabel.Text = item.Name;
            }
            catch (Exception)
            {
                // 読み込んだファイルがXML形式でない場合、
                // エラー・メッセージをラベルに表示して、
                // ［保存］ボタンを無効化します
                fileNameLabel.Text = "ファイルの読み込みに失敗しました";
                saveButton.Enabled = false;
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            // データグリッドビューで編集された内容を
            // XMLファイルに保存します
            _dataSet.WriteXml(_xmlFilePath);
        }
    }
}
