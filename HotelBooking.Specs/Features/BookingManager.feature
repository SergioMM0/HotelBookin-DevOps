Feature: Create Booking

    Scenario: Successfully create a booking with available room
        Given the hotel has 5 rooms available
        When I try to create a booking from "2024-10-22" to "2024-10-23"
        Then the booking should be created successfully

    Scenario: Fail to create a booking when no rooms are available
        Given the hotel has 0 rooms available
        When I try to create a booking from "2024-10-22" to "2024-10-23"
        Then the booking should fail to be created

    Scenario: Fail to create a booking with a start date in the past
        Given the hotel has 1 rooms available
        When I try to create a booking from "2024-10-20" to "2024-10-22"
        Then the booking should fail with an invalid start date error

    Scenario: Fail to create a booking with a start date after the end date
        Given the hotel has 1 rooms available
        When I try to create a booking from "2024-10-23" to "2024-10-22"
        Then the booking should fail with an invalid date range error

    Scenario: Successfully create a booking on the same day
        Given the hotel has 1 rooms available
        When I try to create a booking from "2024-10-22" to "2024-10-22"
        Then the booking should be created successfully