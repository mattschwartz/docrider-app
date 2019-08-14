import React from 'react'
import DocumentControls from '../components/DocumentControls'

import docriderStyle from '../styles/docrider.scss'

class DocumentView extends React.Component {
  constructor (props) {
    super(props)
    this.state = {
      docFile: {
        filepath: './documents/rockin-files/docrider.dr',
        fileSizeBytes: 1591,
        isEdited: false
      },
      docFileStatus: 'changes pending',
      isEditing: false
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

        <div className={docriderStyle.docstats}>
          File: {this.state.docFile.filepath}{' '}
          {this.state.docFile.isEdited && <span>*</span>}
          {this.state.isEditing && <span> [editing]</span>}
          <span className={docriderStyle.fileStats}>
            {this.state.docFile.fileSizeBytes} B
          </span>
        </div>
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
    this.setState({
      ...this.state,
      docFile: {
        ...this.state.docFile,
        isEdited: true
      }
    })
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
      this.setState({
        ...this.state,
        docFile: {
          ...this.state.docFile,
          isEdited: true
        }
      })
    }
  }

  handleKeyPress = (e: KeyboardEvent) => {
    console.log('Pressy', String.fromCharCode(e.charCode))
    this.setState({
      ...this.state,
      docFile: {
        ...this.state.docFile,
        isEdited: true
      }
    })
  }
}

export default DocumentView
