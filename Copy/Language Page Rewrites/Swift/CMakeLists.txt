cmake_minimum_required(VERSION 3.7)
project(SwiftTest)

add_custom_target(SwiftTest
        COMMAND /usr/bin/swift build
        WORKING_DIRECTORY ${CMAKE_SOURCE_DIR}
        SOURCES Sources/main.swift)
