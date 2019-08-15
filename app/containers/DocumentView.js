import React from 'react'
import { Editor, EditorState } from 'draft-js'
import DocumentControls from '../components/DocumentControls'

import docriderStyle from '../styles/docrider.scss'

class DocumentView extends React.Component {
  constructor (props) {
    super(props)
    this.state = { editorState: EditorState.createEmpty() }
    this.onChange = editorState => this.setState({ editorState })
    this.setEditor = editor => {
      this.editor = editor
    }
    this.focusEditor = () => {
      if (this.editor) {
        this.editor.focus()
      }
    }
  }

  componentDidMount () {
    this.focusEditor()
  }

  render () {
    return (
      <div className={docriderStyle.docrider}>
        <DocumentControls onSaveClicked={this.handleOnSave} />
        <div className={docriderStyle.editable} onClick={this.focusEditor}>
          <Editor
            ref={this.setEditor}
            editorState={this.state.editorState}
            onChange={this.onChange}
          />
        </div>
      </div>
    )
  }
}

export default DocumentView
