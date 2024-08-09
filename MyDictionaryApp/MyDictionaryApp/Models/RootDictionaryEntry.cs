namespace MyDictionaryApp.Models;

public class RootDictionaryEntry
{
    public string id { get; set; }
    public Metadata metadata { get; set; }
    public List<Result> results { get; set; }
    public string word { get; set; }
}

public class Metadata
{
    public string operation { get; set; }
    public string provider { get; set; }
    public string schema { get; set; }
}

public class Result
{
    public string id { get; set; }
    public string language { get; set; }
    public List<Lexicalentry> lexicalEntries { get; set; }
    public string type { get; set; }
    public string word { get; set; }
}

public class Lexicalentry
{
    public List<Derivative> derivatives { get; set; }
    public List<DictionaryEntry> entries { get; set; }
    public string language { get; set; }
    public Lexicalcategory lexicalCategory { get; set; }
    public string text { get; set; }
}

public class Lexicalcategory
{
    public string id { get; set; }
    public string text { get; set; }
}

public class Derivative
{
    public string id { get; set; }
    public string text { get; set; }
}

public class DictionaryEntry
{
    public List<string> etymologies { get; set; }
    public List<Pronunciation> pronunciations { get; set; }
    public List<Sens> senses { get; set; }
}

public class Pronunciation
{
    public string audioFile { get; set; }
    public List<string> dialects { get; set; }
    public string phoneticNotation { get; set; }
    public string phoneticSpelling { get; set; }
}

public class Sens
{
    public List<Construction> constructions { get; set; }
    public List<string> definitions { get; set; }
    public List<Example> examples { get; set; }
    public string id { get; set; }
    public List<Semanticclass> semanticClasses { get; set; }
    public List<string> shortDefinitions { get; set; }
    public List<Synonym> synonyms { get; set; }
    public List<Thesauruslink> thesaurusLinks { get; set; }
    public List<Note1> notes { get; set; }
    public List<Subsens> subsenses { get; set; }
    public List<Domainclass1> domainClasses { get; set; }
    public List<Domain> domains { get; set; }
    public List<Variantform> variantForms { get; set; }
}

public class Construction
{
    public string text { get; set; }
}

public class Example
{
    public string text { get; set; }
    public List<Note> notes { get; set; }
}

public class Note
{
    public string text { get; set; }
    public string type { get; set; }
}

public class Semanticclass
{
    public string id { get; set; }
    public string text { get; set; }
}

public class Synonym
{
    public string language { get; set; }
    public string text { get; set; }
}

public class Thesauruslink
{
    public string entry_id { get; set; }
    public string sense_id { get; set; }
}

public class Note1
{
    public string text { get; set; }
    public string type { get; set; }
}

public class Subsens
{
    public List<string> definitions { get; set; }
    public List<Example1> examples { get; set; }
    public string id { get; set; }
    public List<Semanticclass1> semanticClasses { get; set; }
    public List<string> shortDefinitions { get; set; }
    public List<Synonym1> synonyms { get; set; }
    public List<Thesauruslink1> thesaurusLinks { get; set; }
    public List<Domainclass> domainClasses { get; set; }
}

public class Example1
{
    public string text { get; set; }
}

public class Semanticclass1
{
    public string id { get; set; }
    public string text { get; set; }
}

public class Synonym1
{
    public string language { get; set; }
    public string text { get; set; }
}

public class Thesauruslink1
{
    public string entry_id { get; set; }
    public string sense_id { get; set; }
}

public class Domainclass
{
    public string id { get; set; }
    public string text { get; set; }
}

public class Domainclass1
{
    public string id { get; set; }
    public string text { get; set; }
}

public class Domain
{
    public string id { get; set; }
    public string text { get; set; }
}

public class Variantform
{
    public string text { get; set; }
}
