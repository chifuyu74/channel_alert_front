async function ClipboardCopy(text) {
  const type = "text/plain";

  const clipboardItem = new ClipboardItem(
    {
      [type]: text,
    },
    { presentationStyle: "attachment" }
  );
  await navigator.clipboard
    .write([clipboardItem])
    .then(() => {
      window.alert("URL Copied!");
    })
    .catch((reason) => {
      window.alert(`URL Copy Error! ${reason}`);
    });
}
