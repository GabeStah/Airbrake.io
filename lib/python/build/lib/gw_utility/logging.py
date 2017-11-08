import math

from overloading import overload


class Logging:

    separator_character_default = '-'
    separator_length_default = 40

    @classmethod
    def log(cls, value: str):
        cls.output(value)

    @classmethod
    @overload
    def log(cls, value: int):
        cls.output(value)

    @classmethod
    @overload
    def log(cls, value: object):
        cls.output(value)

    @classmethod
    def output(cls, value):
        print(value)

    @classmethod
    def line_separator(cls, value: str, length: int=separator_length_default, char: str=separator_character_default):
        """Output a line separator with inserted text centered in the middle.

        :param value: Inserted text to be centered.
        :param length: Total separator length.
        :param char: Separator character.
        """
        output = value

        if len(value) < length:
            #   Update length based on insert length, less a space for margin.
            length -= len(value) + 2
            #   Halve the length and floor left side.
            left = math.floor(length / 2)
            right = left
            #   If odd number, add dropped remainder to right side.
            if length % 2 != 0:
                right += 1

            #   Surround insert with separators.
            output = f'{char * left} {value} {char * right}'

        cls.output(output)

