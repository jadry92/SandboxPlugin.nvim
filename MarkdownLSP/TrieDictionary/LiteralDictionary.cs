
using System.Text.Json;

namespace TrieDictionary;

public class LiteralDictionary
{

    public Dictionary<string, string>? dictionary;
    private Trie trie;
    private string _fileName = "./data/dictionary_compact.json";


    public LiteralDictionary()
    {
        string filePath = Path.GetFullPath(_fileName);
        if (File.Exists(filePath))
        {
            string text = File.ReadAllText(filePath);
            this.dictionary = JsonSerializer.Deserialize<Dictionary<string, string>>(text);
        }
        else
        {
            throw new FileNotFoundException($"File {filePath} does not exist");
        }
        this.trie = new Trie();
        this.addWordsToTrie();
    }

    public string[] getPrediction(string word)
    {
        string prediction;
        TrieNode node;
        if (word != null)
        {
            (prediction, node) = this.trie.predict(word);
            if (prediction != string.Empty)
            {
                List<Tuple<string, uint>> words = new List<Tuple<string, uint>>();
                this.trie.reconstructWords(node, prediction, words);
                words.Sort((x, y) => y.Item2.CompareTo(x.Item2));
                return words.Select(x => x.Item1).ToArray();
            }
        }
        return [""];
    }

    public string getDefinition(string word)
    {
        if (this.dictionary == null)
        {
            throw new NullReferenceException("Dictionary can not be null");
        }

        if (this.dictionary.ContainsKey(word))
        {
            return this.dictionary[word];
        }
        else
        {
            return "No Definition";

        }
    }

    private void addWordsToTrie()
    {
        if (this.dictionary == null)
        {
            throw new NullReferenceException(" Dictionary can not be null");
        }

        List<string> words = new List<string>(this.dictionary.Keys);

        if (words.Count > 0)
        {
            foreach (string word in words)
            {
                this.trie.add(word);
            }
        }
    }
}
