using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.ComponentModel;

namespace MetaDataManager
{
    public class RoadUnitData
    {
        public int UnitID { get; set; } // 고유식별자
        public string UnitName { get; set; }
        public float MaxHealth { get; set; }
        public float Damage { get; set; }
        public float AttackRate { get; set; } // 초당 공격 횟수
        public float AttackRange { get; set; }
        public float MoveSpeed { get; set; }
    }
    public partial class Form1 : Form
    {
        private BindingList<RoadUnitData> unitList = new BindingList<RoadUnitData> ();
        public Form1()
        {
            // 컴포넌트(화면 구성 요소)들을 초기화(세팅)해라 라는 뜻
            InitializeComponent();
            // UI와 데이터의 결합
            // dataGridView1(UI)는 사용자에게 정보를 보여주는 모니터 화면 엑셀 표 모양임
            // DataSource(결합)는 모니터와 컴퓨터 본체를 연결하는 HDMI 케이블을 꽃는것과 같은 행위임
            dataGridView1.DataSource = unitList;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //try..catch는 일단 해보고, 안 되면 수습해
            //파일을 저장하는 작업은 사고가 자주남 (용량 부족, 권한 없음, 파일 사용 중 등등)
            //그래서 프로그램이 갑자기 꺼지지 않도록 감싸주는 것
            try
            {
                //bulletList라는 C# 객체(메모르에 있는 데이터)를 JSON 형태의 텍스트로 바꾸는 코드
                //컴퓨터 메모리에 있는 데이터 형태 그대로는 파일에 저장할 수 없음 그레서 텍스트 형식인 JSON으로 변환하는 과정이 필요함
                //Formatting.Indented는 가람이 읽기 좋게 들여쓰기를 넣어주는 옵션
                string jsonString = JsonConvert.SerializeObject(unitList, Formatting.Indented);

                //Environment...Desktop은 현재 사용자의 '바탕화면' 폴더 경로를 자동으로 찾아줌
                //Path.Combine은 폴더 경로와 파일 이름(Bullet_MasterData.json)을 안전하게 합쳐줌
                //사용자마다 컴퓨터 이름이 다르기 떄문에, 어떤 컴퓨터에서 실행해도 바탕화면을 정확히 찾아내기 위해 사용
                //string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Road_UnitMasterData.json");

                string path = @"..\..\..\..\..\Assets\StreamingAssets\Road_UnitMasterData.json";
                
                //위에서 만든 주소(path)에, 위에서 변환한 텍스트(jsonString)를 파일로 저장.
                File.WriteAllText(path, jsonString);

                MessageBox.Show("데이터가 완벽하게 저장되었습니다!\n경로: " + path);
            }
            catch (Exception ex)
            {
                MessageBox.Show("저장 실패: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                //string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Road_UnitMasterData.json");
                string path = @"..\..\..\..\..\Assets\StreamingAssets\Road_UnitMasterData.json";

                if (File.Exists(path))
                {
                    string jsonString = File.ReadAllText(path);

                    unitList = JsonConvert.DeserializeObject<BindingList<RoadUnitData>>(jsonString);
                    dataGridView1.DataSource = unitList;

                    MessageBox.Show("데이터를 완벽하게 불러왔습니다!");
                }
                else
                {
                    MessageBox.Show("바탕화면에 불러올 파일(Bullet_MasterData.json)이 없습니다.");
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("불러오기 실패: " + ex.Message);
            }

        }  
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
