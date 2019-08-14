import React from 'react'
import DocumentControls from '../components/DocumentControls'

import docriderStyle from '../styles/docrider.scss'

class DocumentView extends React.Component {
  constructor (props) {
    super(props)
    this.state = {
      docFileStatus: 'changes pending'
    }
  }

  render () {
    return (
      <div className={docriderStyle.docrider}>
        <DocumentControls onSaveClicked={this.handleOnSave} />
        <div
          role='textbox'
          className={docriderStyle.editable}
          onKeyPress={this.handleKeyPress}
          onKeyDown={this.handleKeyDown}
          onPaste={this.handlePaste}
          onFocus={this.handleOnFocus}
          onBlur={this.handleOnBlur}
          autoFocus
          contentEditable
        />
      </div>
    )
  }

  handleOnFocus = () => {
    this.setState({
      ...this.state,
      isEditing: true
    })
  }

  handleOnBlur = e => {
    this.setState({
      ...this.state,
      isEditing: false,
      text: e.target.value
    })
  }

  handleOnSave = () => {
    console.log('Saving: ', this.state.text)
  }

  handlePaste = e => {
    e.preventDefault()
    const text = (e.originalEvent || e).clipboardData.getData('Text')
    document.execCommand('insertHTML', true, text)
  }

  handleKeyDown = (e: KeyboardEvent) => {
    if (e.key === 'Tab') {
      console.log('Key?', window.getSelection().toString())

      if (e.shiftKey) {
        // document.execCommand('insertHTML', false, '&#009')
        // document.execCommand('styleWithCSS', true, null)
        // document.execCommand('outdent', true, null)
      } else {
        document.execCommand('insertHTML', false, '\u0009')
        // document.execCommand('styleWithCSS', true, null)
        // document.execCommand('indent', true, null)
      }

      e.preventDefault()
    }
  }

  handleKeyPress = (e: KeyboardEvent) => {
    console.log('Pressy', String.fromCharCode(e.charCode))
  }
}

export default DocumentView
