using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillItemView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI skillNameText;
    [SerializeField] private TextMeshProUGUI bonusText;
    [SerializeField] private Toggle proficiencyToggle;

    private SkillUIData _skill;

    public event Action<bool> OnProficiencyToggled;

    public void Setup(SkillUIData skill)
    {
        _skill = skill;

        if (skillNameText != null)
            skillNameText.text = skill.Name;

        if (bonusText != null)
            bonusText.text = skill.BonusText;

        if (proficiencyToggle != null)
        {
            proficiencyToggle.isOn = skill.IsProficient;
            proficiencyToggle.onValueChanged.RemoveAllListeners();
            proficiencyToggle.onValueChanged.AddListener(isOn =>
                OnProficiencyToggled?.Invoke(isOn));
        }
    }
}