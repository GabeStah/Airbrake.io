package io.airbrake;

import java.util.ArrayList;

public class ImmutableList<E> {

    private ArrayList<E> list = new ArrayList<>();

    /**
     * Adds a new element to the list.
     * Always throws UnsupportedOperationException.
     *
     * @param element Element to be added.
     * @throws UnsupportedOperationException
     */
    void add(E element) throws UnsupportedOperationException {
        throw new UnsupportedOperationException(
                String.format("%s is immutable, so the element [%s] cannot be added.",
                        this.getClass().getName(),
                        element.toString())
        );
    }

    /**
     * Adds a new element to the list at specified index.
     * Always throws UnsupportedOperationException.
     *
     * @param index Index at which to add element.
     * @param element Element to be added.
     * @throws UnsupportedOperationException
     */
    void add(int index, E element) throws UnsupportedOperationException {
        throw new UnsupportedOperationException(
                String.format("%s is immutable, so the element [%s] cannot be added at index [%d].",
                        this.getClass().getName(),
                        element.toString(),
                        index)
                );
    }

    /**
     * Gets the element at specified index.
     *
     * @param index Index of element.
     * @return Retrieved element.
     */
    public E get(int index) {
        return list.get(index);
    }

    /**
     * Gets the size of list.
     *
     * @return Number of elements in list.
     */
    public int size() {
        return list.size();
    }
}