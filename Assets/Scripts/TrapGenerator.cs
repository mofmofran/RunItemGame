using UnityEngine;

public class TrapGenerator : MonoBehaviour
{

    const float StageChipSize = 30.0f; //ステージの奥行(Z)のサイズ
    public GameObject[] trapPrefabs; //生成するトラップの種類（ここを入れ替える）
    public float minInterval = 5.0f; //トラップを置く間隔(Z)の最小値
    public float maxInterval = 15.0f; //トラップを置く間隔(Z)の最大値
    public float offsetZ = 0.0f; //最初のトラップをどこから置くかのずらし
    public float posY = 1.0f; //トラップを置く高さ

    public float spawnRate = 0.5f;//アイテムが出る確率（0〜1）
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (float z = -StageChipSize / 2 + offsetZ; z < StageChipSize / 2; z += Random.Range(minInterval, maxInterval))
        {
            //確率に外れたら、今回は置かずに次へ
            if (Random.value > spawnRate) continue;

            //配列からランダムにアイテムの種類を選ぶ
            int index = Random.Range(0, trapPrefabs.Length);

            //ステージの位置を基準に、生成する座標を決める
            Vector3 pos = transform.position + new Vector3(transform.position.x, posY, z);

            //ステージの子として生成（ステージが消えると一緒に消える）
            Instantiate(trapPrefabs[index], pos, Quaternion.identity, transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
