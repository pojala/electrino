
console.log("index.js loading")

document.addEventListener('DOMContentLoaded', () => {
	console.log("index.js dom loaded")
	
  let n = new Notification('You did it!', {
    body: 'Nice work.'
  })

  // Tell the notification to show the menubar popup window on click
  n.onclick = () => { ipcRenderer.send('show-window') }

})
