//let video;
//let accesLabel = document.getElementById("accesLabel");

//let label = "waiting...";
//let confidence = "score...";

//let classifier;
//let modelURL = 'https://teachablemachine.withgoogle.com/models/NIbNxWQok/';

//var accessGranted = false;
//var ProvideInfo = false;
//function preload() {
//    classifier = ml5.imageClassifier(modelURL + 'model.json');
//}


//function setup() {
//    createCanvas(640, 520);

//    video = createCapture(VIDEO);
//    video.hide();

//    classifyVideo();
//}


//function classifyVideo() {
//    classifier.classify(video, gotResults);
//}

//function draw() {
//    background(0);

//    // Draw the video
//    image(video, 0, 0);

//    // STEP 4: Draw the label
//    textSize(32);
//    textAlign(CENTER, CENTER);
//    fill(255);
//    text(label, width / 2, height - 16);


//    let emoji = "access denied";
//    if (label == "ring") {
//        emoji = "Ring Approved";
//        accessGranted = true;
//    } else if (label == "Blank") {
//        emoji = "access denied";
//    } else if (label == "Art") {
//        emoji = "access granted";
//        ProvideInfo = true;
//    }
//    else if (label == "Wrong ring") {
//        emoji = "access denied";
//    }
//    if (label == "No ring") {
//        accesLabel.innerText = "wrong code try again";
//    }


//    if (accessGranted == true) {
//        accesLabel.innerText = "Page unlocked";
//    }
//    else if (ProvideInfo == true) {
//        accesLabel.innerText = "Art pls provide Proof";
//    }

//    textSize(20);
//    text(emoji, width / 2, height / 2);
//}


//function gotResults(error, results) {

//    if (error) {
//        console.error(error);
//        return;
//    }

//    label = results[0].label;
//    confidence = results[0].confidence;
//    classifyVideo();
//}