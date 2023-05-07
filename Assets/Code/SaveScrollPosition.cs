using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Util;

public class SaveScrollPosition : MonoBehaviour {
    private ScrollRect scrollRect;

    private void Awake() {
        scrollRect = GetComponent<ScrollRect>();
    }

    public void Load(string key) {
        var data = SaveData.Load();
        var dictionary = data.scrollPositions.ToDictionary(x => x.key, x => x.value);
        var pos = dictionary.TryGetValue(key, out var value) ? value : 0;
        StartCoroutine(ScrollToRoutine(pos));
    }

    public IEnumerator ScrollToRoutine(float position) {
        scrollRect.horizontalNormalizedPosition = .5f;
        yield return null;
        scrollRect.horizontalNormalizedPosition = position;
    }

    public void Save(string key) {
        using var data = SaveData.Load();
        var dictionary = data.scrollPositions.ToDictionary(x => x.key, x => x.value);
        dictionary[key] = scrollRect.horizontalNormalizedPosition;
        data.scrollPositions = dictionary.Select(x => new SaveData.KeyValuePair<string, float> {key = x.Key, value = x.Value}).ToList();
    }
}
