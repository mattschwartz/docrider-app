import React from 'react'

import styles from '../styles/home.scss'

class DocriderStatsBar extends React.Component {
  constructor (props) {
    super(props)
    this.state = {
      filepath: 'Worlds',
      currentAct: 'Act 1',
      currentScene: 'Intro__EvelynnAndJames',
      isEdited: false,
      isEditing: false
    }
  }

  render () {
    return (
      <div className={styles.docstats}>
        ┏━ {this.state.filepath}
        {this.state.isEdited && <span> *</span>}
        {this.state.isEditing && <span> [editing]</span>} ━┓
        <span className={styles.fileStats}>
          ┏━ {this.state.currentAct} ━╥━ {this.state.currentScene} ━┓
        </span>
      </div>
    )
  }
}

export default DocriderStatsBar
