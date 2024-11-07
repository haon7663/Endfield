using TMPro;
using UnityEngine;

public class SkillFrame : MonoBehaviour
{
    [SerializeField] private TMP_Text elixirLabel;

    public void Init(Skill skill)
    {
        elixirLabel.text = skill.elixir.ToString();
    }
}
