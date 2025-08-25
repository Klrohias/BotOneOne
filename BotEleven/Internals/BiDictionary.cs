namespace BotEleven.Internals;

/*
 * From https://stackoverflow.com/a/268545/12568752
 * Modified
 */

/// <summary>
/// This is a dictionary guaranteed to have only one of each value and key. 
/// It may be searched either by TFirst or by TSecond, giving a unique answer because it is 1 to 1.
/// </summary>
/// <typeparam name="TFirst">The type of the "key"</typeparam>
/// <typeparam name="TSecond">The type of the "value"</typeparam>
internal class BiDictionary<TFirst, TSecond>
    where TFirst : notnull
    where TSecond : notnull
{
    private readonly Dictionary<TFirst, TSecond> _firstToSecond = new();
    private readonly Dictionary<TSecond, TFirst> _secondToFirst = new();

    #region Exception throwing methods

    /// <summary>
    /// Tries to add the pair to the dictionary.
    /// Throws an exception if either element is already in the dictionary
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    public void Add(TFirst first, TSecond second)
    {
        if (_firstToSecond.ContainsKey(first) || _secondToFirst.ContainsKey(second))
            throw new ArgumentException("Duplicate first or second");

        _firstToSecond.Add(first, second);
        _secondToFirst.Add(second, first);
    }

    /// <summary>
    /// Find the TSecond corresponding to the TFirst first
    /// Throws an exception if first is not in the dictionary.
    /// </summary>
    /// <param name="first">the key to search for</param>
    /// <returns>the value corresponding to first</returns>
    public TSecond? GetByFirst(TFirst first)
    {
        if (!_firstToSecond.TryGetValue(first, out var second))
            throw new ArgumentException(null, nameof(first));

        return second;
    }

    /// <summary>
    /// Find the TFirst corresponing to the Second second.
    /// Throws an exception if second is not in the dictionary.
    /// </summary>
    /// <param name="second">the key to search for</param>
    /// <returns>the value corresponding to second</returns>
    public TFirst? GetBySecond(TSecond second)
    {
        if (!_secondToFirst.TryGetValue(second, out var first))
            throw new ArgumentException(null, nameof(second));

        return first;
    }


    /// <summary>
    /// Remove the record containing first.
    /// If first is not in the dictionary, throws an Exception.
    /// </summary>
    /// <param name="first">the key of the record to delete</param>
    public void RemoveByFirst(TFirst first)
    {
        if (!_firstToSecond.Remove(first, out var second))
            throw new ArgumentException(null, nameof(first));

        _secondToFirst.Remove(second);
    }

    /// <summary>
    /// Remove the record containing second.
    /// If second is not in the dictionary, throws an Exception.
    /// </summary>
    /// <param name="second">the key of the record to delete</param>
    public void RemoveBySecond(TSecond second)
    {
        if (!_secondToFirst.Remove(second, out var first))
            throw new ArgumentException(null, nameof(second));

        _firstToSecond.Remove(first);
    }

    #endregion

    #region Try methods

    /// <summary>
    /// Tries to add the pair to the dictionary.
    /// Returns false if either element is already in the dictionary        
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns>true if successfully added, false if either element are already in the dictionary</returns>
    public bool TryAdd(TFirst first, TSecond second)
    {
        if (_firstToSecond.ContainsKey(first) || _secondToFirst.ContainsKey(second))
            return false;

        _firstToSecond.Add(first, second);
        _secondToFirst.Add(second, first);
        return true;
    }


    /// <summary>
    /// Find the TSecond corresponding to the TFirst first.
    /// Returns false if first is not in the dictionary.
    /// </summary>
    /// <param name="first">the key to search for</param>
    /// <param name="second">the corresponding value</param>
    /// <returns>true if first is in the dictionary, false otherwise</returns>
    public bool TryGetByFirst(TFirst first, out TSecond? second)
    {
        return _firstToSecond.TryGetValue(first, out second);
    }

    /// <summary>
    /// Find the TFirst corresponding to the TSecond second.
    /// Returns false if second is not in the dictionary.
    /// </summary>
    /// <param name="second">the key to search for</param>
    /// <param name="first">the corresponding value</param>
    /// <returns>true if second is in the dictionary, false otherwise</returns>
    public bool TryGetBySecond(TSecond second, out TFirst? first)
    {
        return _secondToFirst.TryGetValue(second, out first);
    }

    /// <summary>
    /// Remove the record containing first, if there is one.
    /// </summary>
    /// <param name="first"></param>
    /// <returns> If first is not in the dictionary, returns false, otherwise true</returns>
    public bool TryRemoveByFirst(TFirst first)
    {
        if (!_firstToSecond.Remove(first, out var second))
            return false;

        _secondToFirst.Remove(second);
        return true;
    }

    /// <summary>
    /// Remove the record containing second, if there is one.
    /// </summary>
    /// <param name="second"></param>
    /// <returns> If second is not in the dictionary, returns false, otherwise true</returns>
    public bool TryRemoveBySecond(TSecond second)
    {
        if (!_secondToFirst.Remove(second, out var first))
            return false;

        _firstToSecond.Remove(first);
        return true;
    }

    #endregion

    /// <summary>
    /// The number of pairs stored in the dictionary
    /// </summary>
    public int Count => _firstToSecond.Count;

    /// <summary>
    /// Removes all items from the dictionary.
    /// </summary>
    public void Clear()
    {
        _firstToSecond.Clear();
        _secondToFirst.Clear();
    }
}
