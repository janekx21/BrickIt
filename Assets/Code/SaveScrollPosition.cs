using System;
using System.Collections;
using System.Linq;
using Model;
using UnityEngine;
using UnityEngine.UI;
using Util;

public class SaveScrollPosition : MonoBehaviour {
    private ScrollRect scrollRect;

    private void Awake() {
        scrollRect = GetComponent<ScrollRect>();
    }

    public void Load(string key) {
        using var handle = SaveData.GetHandle();
        var dictionary = handle.save.scrollPositions.ToDictionary(x => x.key, x => x.value);
        var pos = dictionary.TryGetValue(key, out var value) ? value : 0;
        StartCoroutine(ScrollToRoutine(pos));
    }

    public IEnumerator ScrollToRoutine(float position) {
        scrollRect.horizontalNormalizedPosition = .5f;
        yield return null;
        scrollRect.horizontalNormalizedPosition = position;
    }

    public void Save(string key) {
        using var handle = SaveData.GetHandle();
        var dictionary = handle.save.scrollPositions.AsDictionary();
        dictionary[key] = scrollRect.horizontalNormalizedPosition;
        handle.save.scrollPositions = dictionary.AsList();
    }
}
