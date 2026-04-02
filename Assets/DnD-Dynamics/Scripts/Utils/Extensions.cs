using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class Extensions
{
    public static void SetTextColorSmooth(this TextMeshProUGUI text, Color color, float duration = Constants.ANIMATION_DURATION)
    {
        if (text != null)
        {
            text.CrossFadeColor(color, duration, false, true);
        }
    }

    public static void SetImageColorSmooth(this Image image, Color color, float duration = Constants.ANIMATION_DURATION)
    {
        if (image != null)
        {
            image.CrossFadeColor(color, duration, false, true);
        }
    }

    public static void ShowAnimated(this GameObject obj, float duration = Constants.ANIMATION_DURATION)
    {
        if (obj != null)
        {
            obj.SetActive(true);
            var canvasGroup = obj.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = obj.AddComponent<CanvasGroup>();
            }

            canvasGroup.alpha = 0;
            canvasGroup.DOFade(1, duration);
        }
    }

    public static void HideAnimated(this GameObject obj, float duration = Constants.ANIMATION_DURATION)
    {
        if (obj != null)
        {
            var canvasGroup = obj.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = obj.AddComponent<CanvasGroup>();
            }

            canvasGroup.DOFade(0, duration, () => obj.SetActive(false));
        }
    }

    public static void DOFade(this CanvasGroup canvasGroup, float targetAlpha, float duration, System.Action onComplete = null)
    {
        if (canvasGroup == null) return;

        var startAlpha = canvasGroup.alpha;
        var startTime = Time.time;

        void Update()
        {
            var elapsed = Time.time - startTime;
            var t = Mathf.Clamp01(elapsed / duration);
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);

            if (t >= 1)
            {
                canvasGroup.alpha = targetAlpha;
                onComplete?.Invoke();
            }
        }

        canvasGroup.alpha = targetAlpha;
        onComplete?.Invoke();
    }

    public static int Clamp(this int value, int min, int max)
    {
        return Mathf.Clamp(value, min, max);
    }

    public static float Clamp(this float value, float min, float max)
    {
        return Mathf.Clamp(value, min, max);
    }

    public static string ToModifierString(this int modifier)
    {
        return modifier >= 0 ? $"+{modifier}" : modifier.ToString();
    }

    public static Color GetHpColor(int currentHp, int maxHp)
    {
        var percent = (float)currentHp / maxHp;

        if (percent <= 0.25f)
            return Constants.DangerColor;
        if (percent <= 0.5f)
            return Constants.WarningColor;

        return Constants.SuccessColor;
    }
}