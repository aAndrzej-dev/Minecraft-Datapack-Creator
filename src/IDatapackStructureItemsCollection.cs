using System.Collections;
using System.Collections.Generic;

namespace MinecraftDatapackCreator;

public interface IDatapackStructureItemsCollection : IList<DatapackStructureItem>, IList
{

    DatapackStructureItem? GetDatapackStructureItemByName(string name);
}