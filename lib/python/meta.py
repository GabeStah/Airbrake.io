class Meta:
    def __init__(self):
        try:
            raise Arith
        except Exception as err:
            thing = type(err)
            print(f'Class: {thing.__name__}, Base: {thing.__bases__[0].__name__}')
            #print("assertion failed")
            #print(err)
        #   self.inheritors(BaseException)

    def inheritors(self, klass):
        subclasses = set()
        work = [klass]
        data = dict()
        data['BaseException'] = {}
        while work:
            parent = work.pop()
            for child in parent.__subclasses__():
                #   if child.__bases__[0] == parent:
                if child not in subclasses:
                    subclasses.add(child)
                    work.append(child)
                    # self.add_to_dict(data, parent.__name__, child.__name__)

        for subclass in subclasses:
            print(subclass.__name__)
        # print(subclasses)

        for blah in data:
            print(blah)

        return subclasses

    def add_to_dict(self, dict, parent, child):
        found = self.find_element(dict, parent, child)
        # if found:
        #     found[child] = child
        # else:
        #     dict[parent] = child

    def find_element(self, obj, key, child):
        if key in obj:
            obj[key][child] = {}
            return obj[key]

        for k, v in obj.items():
            if isinstance(v, dict):
                item = self.find_element(v, key, child)
                if item is not None:
                    v[key][child] = {}
                    return item


def main():
    meta = Meta()


if __name__ == "__main__":
    main()
