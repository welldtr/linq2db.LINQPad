﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using System.Linq;

using CodeJam.Strings;

namespace LinqToDB.LINQPad
{
	static class Pluralization
	{
		static readonly PluralizationService _service = PluralizationService.CreateService(CultureInfo.GetCultureInfo("en"));

		static readonly Dictionary<string,string> _dictionary = new Dictionary<string,string>
		{
			{ "access",       "accesses"     }, { "afterlife",    "afterlives"   }, { "alga",         "algae"        },
			{ "alumna",       "alumnae"      }, { "alumnus",      "alumni"       }, { "analysis",     "analyses"     },
			{ "antenna",      "antennae"     }, { "appendix",     "appendices"   }, { "axis",         "axes"         },
			{ "bacillus",     "bacilli"      }, { "basis",        "bases"        }, { "Bedouin",      "Bedouin"      },
			{ "cactus",       "cacti"        }, { "calf",         "calves"       }, { "cherub",       "cherubim"     },
			{ "child",        "children"     }, { "cod",          "cod"          }, { "cookie",       "cookies"      },
			{ "criterion",    "criteria"     }, { "curriculum",   "curricula"    }, { "data",         "data"         },
			{ "deer",         "deer"         }, { "diagnosis",    "diagnoses"    }, { "die",          "dice"         },
			{ "dormouse",     "dormice"      }, { "elf",          "elves"        }, { "elk",          "elk"          },
			{ "erratum",      "errata"       }, { "esophagus",    "esophagi"     }, { "fauna",        "faunae"       },
			{ "fish",         "fish"         }, { "flora",        "florae"       }, { "focus",        "foci"         },
			{ "foot",         "feet"         }, { "formula",      "formulae"     }, { "fundus",       "fundi"        },
			{ "fungus",       "fungi"        }, { "genie",        "genii"        }, { "genus",        "genera"       },
			{ "goose",        "geese"        }, { "grouse",       "grouse"       }, { "hake",         "hake"         },
			{ "half",         "halves"       }, { "headquarters", "headquarters" }, { "hippo",        "hippos"       },
			{ "hippopotamus", "hippopotami"  }, { "hoof",         "hooves"       }, { "housewife",    "housewives"   },
			{ "hypothesis",   "hypotheses"   }, { "index",        "indices"      }, { "info",         "info"         },
			{ "jackknife",    "jackknives"   },
			{ "knife",        "knives"       }, { "labium",       "labia"        }, { "larva",        "larvae"       },
			{ "leaf",         "leaves"       }, { "life",         "lives"        }, { "loaf",         "loaves"       },
			{ "louse",        "lice"         }, { "magus",        "magi"         }, { "man",          "men"          },
			{ "memorandum",   "memoranda"    }, { "midwife",      "midwives"     }, { "millennium",   "millennia"    },
			{ "moose",        "moose"        }, { "mouse",        "mice"         }, { "nebula",       "nebulae"      },
			{ "neurosis",     "neuroses"     }, { "nova",         "novas"        }, { "nucleus",      "nuclei"       },
			{ "oesophagus",   "oesophagi"    }, { "offspring",    "offspring"    }, { "ovum",         "ova"          },
			{ "ox",           "oxen"         }, { "papyrus",      "papyri"       }, { "passerby",     "passersby"    },
			{ "penknife",     "penknives"    }, { "person",       "people"       }, { "phenomenon",   "phenomena"    },
			{ "placenta",     "placentae"    }, { "pocketknife",  "pocketknives" }, { "process",      "processes"    },
			{ "pupa",         "pupae"        }, { "radius",       "radii"        }, { "reindeer",     "reindeer"     },
			{ "retina",       "retinae"      }, { "rhinoceros",   "rhinoceros"   }, { "roe",          "roe"          },
			{ "salmon",       "salmon"       }, { "scarf",        "scarves"      }, { "self",         "selves"       },
			{ "seraph",       "seraphim"     }, { "series",       "series"       }, { "sheaf",        "sheaves"      },
			{ "sheep",        "sheep"        }, { "shelf",        "shelves"      }, { "species",      "species"      },
			{ "spectrum",     "spectra"      }, { "status",       "status"       }, { "stimulus",     "stimuli"      },
			{ "stratum",      "strata"       }, { "supernova",    "supernovas"   }, { "swine",        "swine"        },
			{ "terminus",     "termini"      }, { "thesaurus",    "thesauri"     }, { "thesis",       "theses"       },
			{ "thief",        "thieves"      }, { "trout",        "trout"        }, { "vulva",        "vulvae"       },
			{ "wife",         "wives"        }, { "wildebeest",   "wildebeest"   }, { "wolf",         "wolves"       },
			{ "woman",        "women"        }, { "yen",          "yen"          },
		};

		static string GetLastWord(string str)
		{
			if (str.IsNullOrWhiteSpace())
				return string.Empty;

			var i       = str.Length - 1;
			var isLower = char.IsLower(str[i]);

			while (i > 0 && char.IsLower(str[i-1]) == isLower)
				i--;

			return str.Substring(isLower && i > 0 ? i - 1 : i);
		}

		public static string ToPlural(string str)
		{
			var word = GetLastWord(str);
	 
			string newWord;
	 
			if (!_dictionary.TryGetValue(word.ToLower(), out newWord))
				newWord = _service.IsPlural(word) ? word : _service.Pluralize(word);

			if (string.Compare(word, newWord, StringComparison.OrdinalIgnoreCase) != 0)
			{
				if (char.IsUpper(word[0]))
					newWord = char.ToUpper(newWord[0]) + newWord.Substring(1, newWord.Length - 1);

				return word == str ? newWord : str.Substring(0, str.Length - word.Length) + newWord;
			}

			return str;
		}

		public static string ToSingular(string str)
		{
			var word = GetLastWord(str);

			var newWord =
				_dictionary
					.Where (dic => string.Compare(dic.Value, word, StringComparison.OrdinalIgnoreCase) == 0)
					.Select(dic => dic.Key)
					.FirstOrDefault()
				??
				(_service.IsSingular(word) ? word : _service.Singularize(word));

			if (string.Compare(word, newWord, StringComparison.OrdinalIgnoreCase) != 0)
			{
				if (char.IsUpper(word[0]))
					newWord = char.ToUpper(newWord[0]) + newWord.Substring(1, newWord.Length - 1);

				return word == str ? newWord : str.Substring(0, str.Length - word.Length) + newWord;
			}

			return str;
		}
	}
}
