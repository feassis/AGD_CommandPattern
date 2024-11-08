using Command.Events;
using Command.Main;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionView : MonoBehaviour
{
    private MinionController controller;
    
    public void SetController(MinionController controller)
    {
        this.controller = controller;   
    }

    public void Died()
    {
        Destroy(gameObject);
    }
}

public class MinionController
{
    private MinionView view;
    private MinionModel model;

    public MinionController(MinionView view, Vector3 spawnPos, MinionModel model, Transform parent)
    {
        this.view = GameObject.Instantiate<MinionView>(view);
        this.view.transform.position = spawnPos;
        this.view.SetController(this);
        this.view.transform.parent = parent;
        this.model = model;
        model.SetControler(this);
    }

    public void Kill()
    {
        view.Died();
    }

    public void TakeDamage(int dmg)
    {
        model.HP -= dmg;

        if(model.HP <= 0)
        {
            view.Died();
            GameService.Instance.EventService.OnMinionDeath.InvokeEvent(this);
        }
    }

    public int GetAttackPower()
    {
        return model.AttackPower;
    }
}

public class MinionModel
{
    public int HP;
    public int AttackPower;
    private MinionController controller;

    public MinionModel(int hP, int attackPower)
    {
        HP = hP;
        AttackPower = attackPower;
    }

    public void SetControler(MinionController controller)
    {
        this.controller = controller;
    }
}
