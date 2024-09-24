using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CupkekGames.Core
{
    [Serializable]
    public class WeightedList<TKey> : KeyValueDatabase<TKey, int>
    {
        private TKey GetValueAtWeight(int weight, List<TKey> keys)
        {
            foreach (TKey key in keys)
            {
                weight -= Dictionary[key];
                if (weight <= 0)
                {
                    return key;
                }
            }

            return default;
        }

        private TKey GetValueAtWeight(int weight) => GetValueAtWeight(weight, Dictionary.Keys.ToList());

        private int GetTotalWeight() => Dictionary.Values.Sum();

        private int GetRandomWeight() => UnityEngine.Random.Range(0, GetTotalWeight());

        public TKey GetRandomItem() => GetValueAtWeight(GetRandomWeight());

        public List<TKey> GetRandomUniqueItems(int numberOfItemsToSelect)
        {
            if (numberOfItemsToSelect >= Dictionary.Count)
            {
                // Return all available items
                return Dictionary.Keys.ToList();
            }

            List<TKey> result = new();
            List<TKey> copiedKeys = new(Dictionary.Keys);

            int totalWeight = GetTotalWeight();

            for (int i = 0; i < numberOfItemsToSelect; i++)
            {
                int randomValue = UnityEngine.Random.Range(0, totalWeight);
                TKey randomKey = GetValueAtWeight(randomValue, copiedKeys);

                result.Add(randomKey);

                copiedKeys.Remove(randomKey);
                totalWeight -= Dictionary[randomKey];
            }

            return result;
        }

        public WeightedList<TKey> GetCopy()
        {
            WeightedList<TKey> copy = new();
            foreach (System.Collections.Generic.KeyValuePair<TKey, int> pair in Dictionary)
            {
                copy.Dictionary.Add(pair.Key, pair.Value);
            }

            return copy;
        }

        public WeightedList<TKey> GetCopyAndCombine(WeightedList<TKey> other)
        {
            WeightedList<TKey> result = GetCopy();

            foreach (System.Collections.Generic.KeyValuePair<TKey, int> pair in other.Dictionary)
            {
                if (result.Dictionary.ContainsKey(pair.Key))
                {
                    result.Dictionary[pair.Key] += pair.Value;
                }
                else
                {
                    result.Dictionary.Add(pair.Key, pair.Value);
                }
            }

            return result;
        }
    }
}
