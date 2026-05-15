using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    public List<RoadUnitData> masterUnitList = new List<RoadUnitData>();

    private void Awake()
    {
        Instance = this;
        LoadUnitData();
    }

    public void LoadUnitData()
    {
        // 바탕화면에서 경로를 찾기
        string jsonPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop), "Road_UnitMasterData.json");

        //괄호 안에 적힌 주소에 파일이 존재(Exists)하는지 검사하는 조건문
        if (File.Exists(jsonPath))
        {
            string jsonString = File.ReadAllText(jsonPath);

            // 유니티 내장 JsonUtility는 배열/리스트를 바로 못 읽으므로 Wrapper를 사용.
            // 만약 JSON이 [ {...} ] 형태라면 아래와 같이 가공이 필요할 수 있음.
            string jsonWrapper = "{ \"unitList\" : " + jsonString + "}";
            // jsonWrapper은 순수한 텍스트
            // JsonUtility.FromJson<...>은 변환기로 유니티에 내장된 기능으로, "이 텍스트를 C# 객체로 변환해줘" 라는 의미 <> 안에는 어떤 모양으로 변환할지 설계도를 적어줌.
            UnitDataWrapper wrapper = JsonUtility.FromJson<UnitDataWrapper>(jsonWrapper);

            masterUnitList = wrapper.unitList;
            Debug.Log("유닛 데이터 로드 완료");
        }
        else
        {
            Debug.LogError("유닛 데이터 파일을 찾을 수 없습니다");
        }
    }

    public RoadUnitData GetUnitDataByID(int id)
    {
        return masterUnitList.Find( u => u.UnitID == id);
    }
    
}
