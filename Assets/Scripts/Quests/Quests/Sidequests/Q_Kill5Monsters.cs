using UnityEngine;

public class Q_Kill5Monsters : Quest
{
    private int startKillcount;

    public override void CheckIfCompleted()
    {
        if (EnemyFight.enemyKillCount >= startKillcount + 5) { CompleteQuest(); }
    }

    public override void AcceptQuest()
    {
        base.AcceptQuest();
        startKillcount = EnemyFight.enemyKillCount;
    }
}
