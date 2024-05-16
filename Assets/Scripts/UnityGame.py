import cv2
import pyautogui
import mediapipe as mp
import time

pyautogui.PAUSE = 0.01

cap = cv2.VideoCapture(0)

mp_hands = mp.solutions.hands
hands = mp_hands.Hands(static_image_mode=False, max_num_hands=2, 
                       min_detection_confidence=0.5, min_tracking_confidence=0.5)

mp_drawing = mp.solutions.drawing_utils

def press_keys_R(index_finger_y, thumb_y):
    if index_finger_y < thumb_y:
        pyautogui.press('d')
    else:
        pyautogui.press('a')


def press_keys_L(index_finger_x, thumb_x):
    if index_finger_x < thumb_x:
         pyautogui.press('w')
    else:
        pyautogui.press('s')


while True:
    ret, frame = cap.read()
    if not ret:
        break

    image_rgb = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)

    results = hands.process(image_rgb)

    if results.multi_hand_landmarks:
        for hand_landmarks, handedness in zip(results.multi_hand_landmarks, results.multi_handedness):
            hand_label = handedness.classification[0].label

            if hand_label == 'Right':
                mp_drawing.draw_landmarks(
                    frame, hand_landmarks, mp_hands.HAND_CONNECTIONS)

                index_finger_x = hand_landmarks.landmark[mp_hands.HandLandmark.INDEX_FINGER_TIP].x
                thumb_x = hand_landmarks.landmark[mp_hands.HandLandmark.THUMB_TIP].x
                if abs(index_finger_x - thumb_x) > 0.1:
                    press_keys_R(index_finger_x, thumb_x)

            elif hand_label == 'Left':
                mp_drawing.draw_landmarks(
                    frame, hand_landmarks, mp_hands.HAND_CONNECTIONS)
                
                index_finger_y = hand_landmarks.landmark[mp_hands.HandLandmark.INDEX_FINGER_TIP].y
                thumb_y = hand_landmarks.landmark[mp_hands.HandLandmark.THUMB_TIP].y
                if abs(index_finger_y - thumb_y) > 0.1:
                    press_keys_L(index_finger_y, thumb_y)

    cv2.imshow('Hand Gesture', frame)

    if cv2.waitKey(1) & 0xFF == ord('q'):
        break

cap.release()
cv2.destroyAllWindows()
