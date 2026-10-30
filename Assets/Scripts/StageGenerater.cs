using UnityEngine;
using System.Collections.Generic;

public class StageGenerater : MonoBehaviour
{
    const int StageChipSize = 30;

    private int _currentChpIndex;

    //public Transform character;
    Transform character;
    public GameObject[] stageChips;
    public int startChipIndex =1 ;
    public int preInstantiate =5 ;

    public List<GameObject> generatedStageList = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        character = GameObject.FindGameObjectWithTag("Player").transform;
        _currentChpIndex = startChipIndex - 1;
        UpdateStage(preInstantiate);
    }

    // Update is called once per frame
    void Update()
    {
        //キャラクターの位置から現在のステージチップのインデックスを計算
        int charaPositionIndex = (int)(character.position.z/StageChipSize);
        //次のステージチップに入ったらステージの更新処理を行う
        if (charaPositionIndex + preInstantiate > _currentChpIndex) UpdateStage(charaPositionIndex + preInstantiate);

    }

    //指定のIndexまでのステージチップを生成して、管理下に置く
    void UpdateStage(int toChipIndex)
    {
        if (toChipIndex <= _currentChpIndex) return;
        //指定のステージチップまでを生成
        for(int i = _currentChpIndex + 1; i <= toChipIndex; i++)
        {
            GameObject stageObject = GenerateStage(i);

            //生成したステージチップを管理リストに追加
            generatedStageList.Add(stageObject);
        }
        //ステージ保持上限内になるまで古いステージを削除
        while (generatedStageList.Count > preInstantiate+2)DestroyOldestStage();
        _currentChpIndex = toChipIndex;
    }
    //指定のインデックス位置にStageオブジェクトをランダムに生成
    GameObject GenerateStage(int chipIndex)
    {
        int nextStageChip = Random.Range(0, stageChips.Length);
        GameObject stageObject = Instantiate(
            stageChips[nextStageChip],
            new Vector3(0, 0, chipIndex * StageChipSize),
            Quaternion.identity);
        return stageObject;
    }
    //一番古いステージを削除
    void DestroyOldestStage()
    {
        GameObject oldstagge = generatedStageList[0];
        generatedStageList.RemoveAt(0);
        Destroy(oldstagge);
    }
}
