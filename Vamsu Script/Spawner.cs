using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;
    public SpawnData[] bossData;

    int level;
    float timer;
    float bossTimer;
    float bossTime = 180f;

    void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();//이거 자기 자신도 포함함

        spawnData = new SpawnData[VamsuGameManager.instance.pool.monsterPrefabs.Length * 5];

        /*spawnData[0].spawnTime = 0.4f;
        spawnData[0].spriteType = 0;
        spawnData[0].health = 10;
        spawnData[0].speed = 1.5f;
        spawnData[0].damage = 3;*/
        
        for(int i=0; i< VamsuGameManager.instance.pool.monsterPrefabs.Length * 5; i++)
        {
            spawnData[i] = new SpawnData();
            spawnData[i].spawnTime = Mathf.Max(0.3f - 0.002f * i, 0.25f);
            spawnData[i].spriteType = i % 5;
            spawnData[i].health = 12 + 2.5f * i;
            spawnData[i].speed = 1.8f + 0.002f * i;
            spawnData[i].damage = 3 + 0.3f * i;
        }

    }

    void Update()
    {
        if (!VamsuGameManager.instance.isLive || VamsuGameManager.instance.gameTime >= VamsuGameManager.instance.maxGameTime)
            return;

        timer += Time.deltaTime;
        bossTimer += Time.deltaTime;

        //현재는 10초당 몬스터가 강해지는 구조 -> 나중에 킬 수랑 타이머 둘 다 써도 괜찮을 듯?
        level = Mathf.FloorToInt(VamsuGameManager.instance.gameTime / 30f);//30f * 4

        if (timer > spawnData[level].spawnTime)
        {
            timer = 0;
            Spawn();
        }

        if (bossTimer > bossTime)
        {
            bossTimer = 0;
            SpawnBoss();
        }

        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnBoss();
        }*/
    }

    void Spawn()
    {
        GameObject enemy = VamsuGameManager.instance.pool.Get(Mathf.FloorToInt(level / 5), true);//
        
        
        int random = 0;
        float dirX = VamsuGameManager.instance.player.inputVec.x;
        float dirZ = VamsuGameManager.instance.player.inputVec.z;

        if(dirX > 0 && dirZ == 0)
        {
            random = Random.Range(2, 7);
        }
        else if (dirX < 0 && dirZ == 0)
        {
            random = Random.Range(9, 14);
        }
        else if (dirX == 0 && dirZ > 0)
        {
            random = Random.Range(1, 5);
            if (random == 3)
                random = 14;
        }
        else if (dirX == 0 && dirZ < 0)
        {
            random = Random.Range(6, 12);
        }
        else if (dirX > 0 && dirZ > 0)
        {
            random = Random.Range(1, 7);
        }
        else if (dirX > 0 && dirZ < 0)
        {
            random = Random.Range(3, 9);
        }
        else if (dirX < 0 && dirZ < 0)
        {
            random = Random.Range(7, 12);
        }
        else if (dirX < 0 && dirZ > 0)
        {
            random = Random.Range(11, spawnPoint.Length);
        }
        else if (dirX == 0 && dirZ == 0)
        {
            random = Random.Range(1, spawnPoint.Length);
        }


        //자기자신 빼려고 1부터
        enemy.transform.position = spawnPoint[random].position + new Vector3(0,-0.98f,0);
        enemy.GetComponent<Enemy>().Init(spawnData[level]);
    }

    void SpawnBoss()
    {
        VamsuSoundManager.instance.PlayBossBGM();

        GameObject boss = VamsuGameManager.instance.pool.GetBoss(0);//
        //level이나 시간에 따라 따로 설정해주자
        //6 12 18 24
        BossData data = new BossData();
        data.health = 80 * level;
        data.speed = 8;
        data.damage = 1 * level;
        data.eggCount = 4 + Mathf.FloorToInt(level * 0.3f);

        //자기자신 빼려고 1부터
        boss.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position + new Vector3(0, -0.98f, 0);
        boss.GetComponent<VamsuBoss>().Init(data);
    }
}

[System.Serializable]
public class SpawnData
{
    public float spawnTime;//얘는 여기서만 씀

    public int spriteType;//level관련
    public float health;
    public float speed;

    public float damage;
}

public class BossData
{
    public float health;
    public float speed;
    public float damage;
    public int eggCount;
}