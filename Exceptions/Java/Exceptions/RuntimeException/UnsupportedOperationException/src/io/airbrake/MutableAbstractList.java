package io.airbrake;

import java.util.AbstractList;
import java.util.ArrayList;

public class MutableAbstractList<E> extends AbstractList<E> {

    private ArrayList<E> list = new ArrayList<>();

    @Override
    public void add(int index, E element) {
        list.add(index, element);
    }

    @Override
    public E get(int index) {
        return list.get(index);
    }

    @Override
    public int size() {
        return list.size();
    }
}