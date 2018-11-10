<template>
  <div class="pdfSelector">
    <div class="column">
      <label>
        Available mission<span v-if="pdfs.length>1">s</span>
      </label>
      <ul>
        <draggable v-model="pdfs" class="dragArea" :options="{group:'pdfs'}">
          <div v-for="pdf in pdfs">
            {{pdf.name}}
          </div>
        </draggable>
      </ul>
    </div>
    <div class="column">
      <label>
        Chosen mission<span v-if="chosen.length>1">s</span>
        <span>
          <a v-on:click="combinePDF">Get mission pack</a>
        </span>
      </label>
      <draggable v-model="chosen" class="dragArea" :options="{group:'pdfs'}">
        <div v-for="pdf in chosen">
          {{pdf.name}}
        </div>
      </draggable>
      <div></div>
    </div>
  </div>
</template>
<script>
  import axios from 'axios'
  import draggable from 'vuedraggable'
  export default {
    components: {
      draggable,
    },
    name: 'pdf-Selector',
    data() {
      return {
        pdfs: [],
        chosen: [],
      }
    },
    methods: {
      combinePDF: function () {
        axios.post('/api/pdf/combined', this.chosen).then(function (response) {
          var headers = response.headers;
          var blob = new Blob([response.data], { type: headers['content-type'] });

          if (window.navigator.msSaveBlob) {
            window.navigator.msSaveBlob(blob, 'export.pdf'); // IE doesnot understand download header
          } else {
            var link = document.createElement('a');
            link.href = window.URL.createObjectURL(blob);
            link['download'] = "Export";
            link.click();
          }
        })
      }
    },
    created() {
      axios
        .get('/api/pdf/list').then(resp => {
          console.log(resp.data)
          //this.pdfs = resp.data
          this.pdfs = [{ "fullPath": "C:\\Users\\fjh\\source\\repos\\InfinityPDF\\Infinity\\PDF\\BasicRules.pdf", "category": [], "name": "Basic Rules" }, { "fullPath": "C:\\Users\\fjh\\source\\repos\\InfinityPDF\\Infinity\\PDF\\Extra\\ArmyLists.pdf", "category": ["Extra"], "name": "Army Lists" }, { "fullPath": "C:\\Users\\fjh\\source\\repos\\InfinityPDF\\Infinity\\PDF\\Extra\\BasicRules.pdf", "category": ["Extra"], "name": "Basic Rules" }, { "fullPath": "C:\\Users\\fjh\\source\\repos\\InfinityPDF\\Infinity\\PDF\\Extra\\ClassifiedObjectives.pdf", "category": ["Extra"], "name": "Classified Objectives" }, { "fullPath": "C:\\Users\\fjh\\source\\repos\\InfinityPDF\\Infinity\\PDF\\Extra\\Extras.pdf", "category": ["Extra"], "name": "Extras" }, { "fullPath": "C:\\Users\\fjh\\source\\repos\\InfinityPDF\\Infinity\\PDF\\Extra\\ITSRating.pdf", "category": ["Extra"], "name": "I T S Rating" }, { "fullPath": "C:\\Users\\fjh\\source\\repos\\InfinityPDF\\Infinity\\PDF\\Extra\\Season10.pdf", "category": ["Extra"], "name": "Season 1 0" }, { "fullPath": "C:\\Users\\fjh\\source\\repos\\InfinityPDF\\Infinity\\PDF\\Extra\\TournamentBasicRules.pdf", "category": ["Extra"], "name": "Tournament Basic Rules" }, { "fullPath": "C:\\Users\\fjh\\source\\repos\\InfinityPDF\\Infinity\\PDF\\Missions\\Acquisition.pdf", "category": ["Missions"], "name": "Acquisition" }, { "fullPath": "C:\\Users\\fjh\\source\\repos\\InfinityPDF\\Infinity\\PDF\\Missions\\Annihilation.pdf", "category": ["Missions"], "name": "Annihilation" }, { "fullPath": "C:\\Users\\fjh\\source\\repos\\InfinityPDF\\Infinity\\PDF\\Missions\\Biotechvore.pdf", "category": ["Missions"], "name": "Biotechvore" }, { "fullPath": "C:\\Users\\fjh\\source\\repos\\InfinityPDF\\Infinity\\PDF\\Missions\\CaptureAndProtect.pdf", "category": ["Missions"], "name": "Capture And Protect" }, { "fullPath": "C:\\Users\\fjh\\source\\repos\\InfinityPDF\\Infinity\\PDF\\Missions\\CommsCenter.pdf", "category": ["Missions"], "name": "Comms Center" }, { "fullPath": "C:\\Users\\fjh\\source\\repos\\InfinityPDF\\Infinity\\PDF\\Missions\\DeadlyDance.pdf", "category": ["Missions"], "name": "Deadly Dance" }, { "fullPath": "C:\\Users\\fjh\\source\\repos\\InfinityPDF\\Infinity\\PDF\\Missions\\Decapitation.pdf", "category": ["Missions"], "name": "Decapitation" }, { "fullPath": "C:\\Users\\fjh\\source\\repos\\InfinityPDF\\Infinity\\PDF\\Missions\\EngineeringDeck.pdf", "category": ["Missions"], "name": "Engineering Deck" }, { "fullPath": "C:\\Users\\fjh\\source\\repos\\InfinityPDF\\Infinity\\PDF\\Missions\\Firefight.pdf", "category": ["Missions"], "name": "Firefight" }, { "fullPath": "C:\\Users\\fjh\\source\\repos\\InfinityPDF\\Infinity\\PDF\\Missions\\Frontline.pdf", "category": ["Missions"], "name": "Frontline" }, { "fullPath": "C:\\Users\\fjh\\source\\repos\\InfinityPDF\\Infinity\\PDF\\Missions\\Frostbyte.pdf", "category": ["Missions"], "name": "Frostbyte" }]
        })
    }
  }
</script>
<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
  .normal {
    background-color: grey;
  }

  .drag {
    background-color: green;
  }

  .dragArea:empty {
    min-height: 10px;
    border: 1px dashed #cecece;
    background: efefefe;
  }

  .pdfSelector {
    display: flex;
  }

  .column {
    width: 25%;
  }
</style>
