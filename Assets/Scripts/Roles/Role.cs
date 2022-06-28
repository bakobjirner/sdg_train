using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Role
{
    private string role;
    private string objective;

    public bool alive = true;
    public string deathReason = "";
    public bool objectiveComplete = false;

    public Role(string role, string objective)
    {
        this.role = role;
        this.objective = objective;
    }

    public string getRole()
    {
        return role;
    }
    public string getObjective()
    {
        return objective;
    }
}
