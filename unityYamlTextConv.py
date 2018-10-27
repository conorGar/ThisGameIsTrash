#!/usr/bin/env python

import sys
import yaml
import pprint
import re
import io

if len(sys.argv) < 2:
	sys.exit(-1)

def tag_unity3d_com_ctor(loader, tag_suffix, node):
	values = loader.construct_mapping(node, deep=True)
	if 'Prefab' in values:
		if 'm_Modification' in values['Prefab']:
			del values['Prefab']['m_Modification']
	return values

class UnityParser(yaml.parser.Parser):
    DEFAULT_TAGS = {u'!u!': u'tag:unity3d.com,2011'}
    DEFAULT_TAGS.update(yaml.parser.Parser.DEFAULT_TAGS)

class MyPrettyPrinter(pprint.PrettyPrinter):
    def format(self, object, context, maxlevels, level):
        if isinstance(object, str):
            return (object, True, False)
        return pprint.PrettyPrinter.format(self, object, context, maxlevels, level)
	
class UnityLoader(yaml.reader.Reader, yaml.scanner.Scanner, UnityParser, yaml.composer.Composer, yaml.constructor.Constructor, yaml.resolver.Resolver):
    def __init__(self, stream):
        yaml.reader.Reader.__init__(self, stream)
        yaml.scanner.Scanner.__init__(self)
        UnityParser.__init__(self)
        yaml.composer.Composer.__init__(self)
        yaml.constructor.Constructor.__init__(self)
        yaml.resolver.Resolver.__init__(self)

UnityLoader.add_multi_constructor('tag:unity3d.com', tag_unity3d_com_ctor)
with open(sys.argv[1], 'r') as ystream:
	data = ystream.read();
	data2 = re.sub('(---.*?) stripped', '\\1', data)
#	open('temp.txt', 'w').write(data2);
	stream = io.StringIO(data2)
	docs = yaml.load_all(stream, Loader=UnityLoader)
	for doc in docs:
		pprint.pprint(doc, width=120, indent=1, depth=6)

#with open(sys.argv[1], 'r', encoding="utf-8") as ystream:
#	data = ystream.read();
#	data2 = re.sub('(---.*?) stripped', '\\1', data)
#	stream = io.StringIO(data2)
#	docs = yaml.load_all(stream, Loader=UnityLoader)
#	logFile = open('temp.txt', 'w')
#	pp = MyPrettyPrinter(width=120, indent=1, depth=6, stream=logFile)
#	pp.pprint(["Encoding is", sys.stdin.encoding])
#	for doc in docs:
#		pp.pprint(doc)